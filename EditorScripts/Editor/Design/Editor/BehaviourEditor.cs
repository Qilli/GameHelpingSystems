using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Linq;
using System.Reflection;


namespace Base.AI.Behaviours.Editor
{
    public class BehaviourEditor : EditorWindow
    {
        [NonSerialized]
        private BehaviourTree selectedBehaviour = null;
        [NonSerialized]
        private BehaviourTreeController runtimeController = null;
        [NonSerialized]
        private GUIStyle nodeStyle;
        [NonSerialized]
        //current node for draging node on grid
        private BehaviourTreeTask currentNode;
        [NonSerialized]
        private BehaviourTreeTask addNodeFor = null;
        [NonSerialized]
        private BehaviourTreeTask nodeToRemove = null;
        [NonSerialized]
        private BehaviourTreeTask linkingNode = null;
        [NonSerialized]
        private Vector2 localOffsetForDrag;
        [NonSerialized]
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 scrollPositionLeftPanel = Vector2.zero;
        [NonSerialized]
        private bool draggingCanvas = false;
        private Vector2 draggingOffset = Vector2.zero;

        private const float canvasSize = 4000.0f;
        private readonly Vector2 leftPanelSize = new Vector2(350, 900);
        private const float bgTextureSize = 50.0f;
        private const float topPanelHeight = 22;
        [NonSerialized]
        private Rect groupRect;
        [NonSerialized]
        private float scaling = 1.0f;
        [NonSerialized]
        private Vector2 mousePosition;
        private SerializedObject selectedSerializedAction = null;
        private List<SerializedProperty> currentProperties = new List<SerializedProperty>();

        //Data for selected node
        //selected node for modification
        [NonSerialized]
        private BehaviourTreeTask selectedNode = null;
        private enum InspectorType { INSPECTOR, TASKS, PARAMETERS };
        [NonSerialized]
        private bool reparenting = false;
        [NonSerialized]
        private BehaviourTreeTask selectedForReparenting = null;
        [NonSerialized]
        private InspectorType selectedInspector = InspectorType.INSPECTOR;
        private List<BehaviourTreeTask> behaviourTasks = new List<BehaviourTreeTask>();
        private List<CompositeTreeTask> behaviourTasks_Composite = new List<CompositeTreeTask>();
        private List<DecoratorTreeTask> behaviourTasks_Decorators = new List<DecoratorTreeTask>();
        private List<ActionTreeTask> behaviourTasks_Actions = new List<ActionTreeTask>();
        private List<SharedVariable> allSharedVariablesTypes = new List<SharedVariable>();
        private string[] sharedVariablesNamesArray;
        [NonSerialized]
        private int currentlySelectedParameterType = 0;
        [NonSerialized]
        private string newSharedVariableName = "NewVariable";
        [NonSerialized]
        private List<string> sharedVariablesToRemove = new List<string>();

        [MenuItem("Window/Behaviour Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(BehaviourEditor), false, "Behaviour Editor", true);
        }

        /// <summary>
        /// Called when we select new asset in editor
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            BehaviourTree behaviour = EditorUtility.InstanceIDToObject(instanceID) as BehaviourTree;//for AS of types dont fit, we get null
            if (behaviour != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += onSelectionChanged;

            //style for node
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(10, 10, 10, 10);
            nodeStyle.border = new RectOffset(10, 10, 10, 10);
            onSelectionChanged();

            getAllTasks();
            getAllSharedVariables();
        }

        private void OnGUI()
        {
            if (selectedBehaviour == null)
            {
                EditorGUILayout.LabelField("No behaviour tree selected");
            }
            else
            {
                processEvents();

                GUILayout.BeginVertical();

                drawTopPanel();

                GUILayout.BeginHorizontal();

                drawLeftPanel();

                // drawTreeArea();
                DrawScrollView();

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                if (addNodeFor != null)
                {
                    //add empty child node for selected node      
                    selectedBehaviour.createNode(addNodeFor, Event.current.mousePosition);
                    addNodeFor = null;
                }

                if (nodeToRemove != null)
                {
                    //remove selected node and all his connections  
                    Undo.RegisterCompleteObjectUndo(selectedBehaviour, "Remove objects");
                    selectedBehaviour.removeNode(nodeToRemove);
                    EditorUtility.SetDirty(selectedBehaviour);
                    nodeToRemove = null;
                }
            }
        }

        private void getAllTasks()
        {
            behaviourTasks = Base.CommonCode.Common.FindAssetsByType<BehaviourTreeTask>();
            List<CompositeTreeTask> allData_Composites = Base.CommonCode.Common.FindAssetsByType<CompositeTreeTask>();
            List<DecoratorTreeTask> allData_Decorators = Base.CommonCode.Common.FindAssetsByType<DecoratorTreeTask>();
            behaviourTasks_Composite.Clear();
            behaviourTasks_Decorators.Clear();
            behaviourTasks_Actions.Clear();
            foreach (CompositeTreeTask t in allData_Composites)
            {
                if (t.useAsTaskSourceEditor)
                {
                    behaviourTasks_Composite.Add(t);
                }
            }
            foreach (DecoratorTreeTask t in allData_Decorators)
            {
                if (t.useAsTaskSourceEditor)
                {
                    behaviourTasks_Decorators.Add(t);
                }
            }
            foreach (BehaviourTreeTask t in behaviourTasks)
            {
                ActionTreeTask current = t as ActionTreeTask;
                if (current!=null && current.useAsTaskSourceEditor)
                {
                    behaviourTasks_Actions.Add(current);
                }
            }
        }

        private bool tryAddNewParameterToBlackboard(string name, int type)
        {
            if(!selectedBehaviour.Blackboard.alreadyContains(name))
            {
                SharedVariable newVariable = Activator.CreateInstance(allSharedVariablesTypes[type].GetType()) as SharedVariable;
                return selectedBehaviour.Blackboard.addNewVariable(name, newVariable);
            }return false;
        }

      /*  IEnumerable<BaseClass> GetAll()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BaseClass)))
                .Select(type => Activator.CreateInstance(type) as BaseClass);
        }*/

        private void getAllSharedVariables()
        {
            Assembly[] assemblies=AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly asm in assemblies)
            {
                foreach(Type t in asm.GetTypes())
                {
                    if(t.IsSubclassOf(typeof(SharedVariable)) && t.IsGenericType==false)
                    {
                        SharedVariable sv = Activator.CreateInstance(t) as SharedVariable;
                      allSharedVariablesTypes.Add(sv);
                    }
                }
            }

            sharedVariablesNamesArray = new string[allSharedVariablesTypes.Count];
            int counter = 0;
            foreach(SharedVariable sv in allSharedVariablesTypes)
            {
                sharedVariablesNamesArray[counter++] = sv.typeName;
            }
        }

        #region ZOOM UI
        ///Scaling helpers
        ///
        private void ProcessScrollWheel(Event e)
        {
            if (e == null || e.type != EventType.ScrollWheel)
                return;
            if (e.control)
            {
                float shiftMultiplier = e.shift ? 4 : 1;
                scaling = Mathf.Clamp(scaling - e.delta.y * 0.01f * shiftMultiplier, 0.5f, 2f);
                e.Use();
            }

        }
        private void ScaleScrollGroup()
        {
            GUI.EndGroup();
            CalculateScaledScrollRect();
            GUI.BeginGroup(groupRect);
        }
        private void CalculateScaledScrollRect()
        {
            groupRect.x = (-scrollPosition.x) / scaling;
            groupRect.y = -scrollPosition.y / scaling;
            groupRect.width = ((position.width - leftPanelSize.x) + scrollPosition.x - GUI.skin.verticalScrollbar.fixedWidth) / scaling;
            groupRect.height = (position.height + scrollPosition.y - 21 - GUI.skin.horizontalScrollbar.fixedHeight) / scaling;
        }
        private void ScaleWindowGroup()
        {
            GUI.EndGroup();
            CalculateScaledWindowRect();
            GUI.BeginGroup(groupRect);
        }
        private void CalculateScaledWindowRect()
        {
            groupRect.x = leftPanelSize.x;
            groupRect.y = 21 + topPanelHeight;
            groupRect.width = (this.position.width - leftPanelSize.x - 30.0f/*+ scrollPosition.x*/) / scaling;
            groupRect.height = (this.position.height - 21.0f /*+ scrollPosition.y*/) / scaling;
        }
        #endregion

        #region DRAW UI
        /// <summary>
        /// DRAW UI
        /// </summary>
        private void DrawNode(BehaviourTreeTask node)
        {

            Color lastColor = GUI.backgroundColor;
            if (selectedNode == node)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = lastColor;
            }
            if (node.taskType != TaskType.ROOT)
            {
                Rect upperPartRect = new Rect(node.NodeRect.x + node.NodeRect.width * 0.5f - node.NodeRect.width * 0.3f, node.NodeRect.y - node.NodeRect.height * 0.1f,
                    node.NodeRect.width * 0.6f, node.NodeRect.height * 0.3f);
                GUILayout.BeginArea(upperPartRect, nodeStyle);

                GUILayout.EndArea();
            }

            if (node.taskType != TaskType.ACTION)
            {
                Rect lowerPartRect = new Rect(node.NodeRect.x + node.NodeRect.width * 0.5f - node.NodeRect.width * 0.3f, node.NodeRect.y + node.NodeRect.height * 0.8f,
                    node.NodeRect.width * 0.6f, node.NodeRect.height * 0.3f);
                GUILayout.BeginArea(lowerPartRect, nodeStyle);

                GUILayout.EndArea();
            }

            GUILayout.BeginArea(node.NodeRect, nodeStyle);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginVertical();

            GUILayout.Label(node.taskTypeName);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box(node.editorTexture);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();


            GUILayout.EndArea();

            if (node.taskType != TaskType.ROOT)
            {
                Rect nodeRectFull = node.NodeRect;
                Rect buttonRect = new Rect(nodeRectFull.x-10.0f+nodeRectFull.width*0.5f,nodeRectFull.y +nodeRectFull.height*0.7f,20.0f,20.0f);
                //buttons for erasing
                if(GUI.Button(buttonRect, "x"))
                {
                    //delete node
                   if(!reparenting) nodeToRemove = node;
                }
                //draw option for reparenting
                Rect reparentRect = new Rect(nodeRectFull.x - 10.0f + nodeRectFull.width * 0.8f, nodeRectFull.y + nodeRectFull.height * 0.7f, 20.0f, 20.0f);
                if (reparenting)
                {
                    if(selectedForReparenting == node)
                    {
                        var oldColor = GUI.backgroundColor;
                        GUI.backgroundColor = Color.red;
                        if (GUI.Button(reparentRect, "R"))
                        {
                            reparenting = false;
                            selectedForReparenting = null;
                        }
                        GUI.backgroundColor = oldColor;         
                    }
                    else
                    {
                        var oldColor = GUI.backgroundColor;
                        GUI.backgroundColor = Color.green;
                        if (GUI.Button(reparentRect, "R"))
                        {
                            BehaviourTree.Reparent(node, selectedForReparenting);
                            reparenting = false;
                            selectedForReparenting = null;
                        }
                        GUI.backgroundColor = oldColor;

                    }
                }
                else
                {
                    if (GUI.Button(reparentRect, "R"))
                    {
                        reparenting = true;
                        selectedForReparenting = node;
                    }
                }

            }

            GUI.backgroundColor = lastColor;
        }
        private void DrawScrollView()
        {
            Event e = Event.current;
            mousePosition = (e.mousePosition + scrollPosition) / scaling;
            ProcessScrollWheel(e);

            ScaleWindowGroup();
            Rect last = GUILayoutUtility.GetLastRect();
            last.x = 0;
            last.y = 0;
            last.width = this.position.width - leftPanelSize.x - 50.0f;
            last.height = this.position.width - 21.0f - 30.0f;
            scrollPosition = GUI.BeginScrollView(last, scrollPosition, new Rect(0, 0, canvasSize * scaling, canvasSize * scaling), false, false);
            ScaleScrollGroup();
            Matrix4x4 old = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(new Vector3(leftPanelSize.x, 21, 1), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(scaling, scaling, scaling));
            GUI.matrix = translation * scale * translation.inverse;
            GUILayout.BeginArea(new Rect(0, 0, canvasSize * scaling * 10, canvasSize * scaling * 10));

            drawTreeArea();

            GUILayout.EndArea();

            // Restore the matrix.
            GUI.matrix = old;
            // Stop the scrollable view.
            GUI.EndScrollView();

            GUI.EndGroup();

            scrollPosition.y = GUI.VerticalScrollbar(new Rect(this.position.width - GUI.skin.verticalScrollbar.fixedWidth * 2, 31.0f, GUI.skin.verticalScrollbar.fixedWidth, this.position.height - 31.0f),
               scrollPosition.y, 200.0F, 0, (canvasSize * scaling) - last.height);

            scrollPosition.x = GUI.HorizontalScrollbar(new Rect(leftPanelSize.x, this.position.height, this.position.width - leftPanelSize.x - GUI.skin.verticalScrollbar.fixedWidth * 2, GUI.skin.horizontalScrollbar.fixedHeight),
                scrollPosition.x, 200.0F, 0.0F, (canvasSize * scaling) - last.width);

            GUI.BeginGroup(new Rect(0, 21, position.width, position.height));
        }
        private void drawLeftPanel()
        {
            scrollPositionLeftPanel = GUILayout.BeginScrollView(scrollPositionLeftPanel, true, true, GUILayout.Width(leftPanelSize.x), GUILayout.Height(position.height - topPanelHeight));

            GUILayout.BeginVertical();

            if (selectedInspector == InspectorType.TASKS)
            {
                onDrawTasksPanel();
            }
            else if(selectedInspector == InspectorType.INSPECTOR)
            {
                onDrawInspectorPanel();
                EditorUtility.SetDirty(selectedBehaviour);
            }
            else if(selectedInspector == InspectorType.PARAMETERS)
            {
                onDrawParametersPanel();
                EditorUtility.SetDirty(selectedBehaviour);
            }

            GUILayout.EndVertical();

            GUILayout.EndScrollView();

        }

        private void onDrawParametersPanel()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Global Parameters");
            drawAddNewSharedVariablePanel();
            drawSharedVariablesList();
            GUILayout.EndVertical();
        }
        private void drawSharedVariablesList()
        {
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
            sharedVariablesToRemove.Clear();
            foreach (SharedVariable sv in selectedBehaviour.Blackboard.getAllSharedVariables())
            {
                drawSingleSharedVariable(sv);
            }

            GUILayout.EndVertical();

            //check if we have any to erase
            foreach(string elem in sharedVariablesToRemove)
            {
                selectedBehaviour.Blackboard.removeVariableByName(elem);
            }
        }
        private void drawSingleSharedVariable(SharedVariable sv)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(sv.name);
            drawSharedVariableInput(sv);
            if(GUILayout.Button("X",GUILayout.Width(20),GUILayout.Height(20)))
            {
                sharedVariablesToRemove.Add(sv.name);
            }
            GUILayout.EndHorizontal();
        }

        private void drawSharedVariableInput(SharedVariable sv)
        {
            if(sv.type == SharedVariable.SharedType.TRANSFORM)
            {
                SharedTransform sv_transform = sv as SharedTransform;
                sv_transform.value=EditorGUILayout.ObjectField(sv_transform.value, typeof(Transform), false) as Transform;
            }
            else if(sv.type == SharedVariable.SharedType.STRING)
            {
                SharedString sv_string = sv as SharedString;
                sv_string.value = GUILayout.TextField(sv_string.value);
            }
            else if(sv.type == SharedVariable.SharedType.GAMEOBJECT)
            {
                SharedGameObject sv_gameobject = sv as SharedGameObject;
                sv_gameobject.value = EditorGUILayout.ObjectField(sv_gameobject.value, typeof(GameObject), false) as GameObject;
            }
            else if (sv.type == SharedVariable.SharedType.OBJECT)
            {
                SharedObject sv_object = sv as SharedObject;
                sv_object.value = EditorGUILayout.ObjectField(sv_object.value, typeof(UnityEngine.Object), false) as UnityEngine.Object;
            }
            else if(sv.type == SharedVariable.SharedType.FLOAT)
            {
                SharedFloat sv_float = sv as SharedFloat;
                sv_float.value = EditorGUILayout.FloatField(sv_float.value);
            }
            else if(sv.type == SharedVariable.SharedType.INT)
            {
                SharedInt sv_integer = sv as SharedInt;
                sv_integer.value = EditorGUILayout.IntField(sv_integer.value);
            }
            else if (sv.type == SharedVariable.SharedType.BOOL)
            {
                SharedBool sv_bool = sv as SharedBool;
                sv_bool.value = EditorGUILayout.Toggle(sv_bool.value);
            }
            else if (sv.type == SharedVariable.SharedType.VECTOR)
            {
                SharedVector sv_vec = sv as SharedVector;
                sv_vec.value = EditorGUILayout.Vector3Field("",sv_vec.value);
            }
        }

        private void drawAddNewSharedVariablePanel()
        {
            GUILayout.BeginHorizontal();
            //type of shared variable
            GUILayout.Label("New param: ");
            currentlySelectedParameterType = EditorGUILayout.Popup(currentlySelectedParameterType, sharedVariablesNamesArray);
            newSharedVariableName=GUILayout.TextField(newSharedVariableName,GUILayout.MinWidth(100));
            if(GUILayout.Button("Add"))
            {
                if(!tryAddNewParameterToBlackboard(newSharedVariableName,currentlySelectedParameterType))
                {
                    this.ShowNotification(new GUIContent("Parametr with specifed name already exist!"));
                }
            }
            GUILayout.EndHorizontal();
        }
        private void onDrawInspectorPanel()
        {
            if (selectedNode == null) return;
            if (selectedNode.taskType == TaskType.ACTION)
            {
                ActionTreeTask actionTask = selectedNode as ActionTreeTask;
                if (actionTask != null && selectedSerializedAction==null)
                {
                    //create serialized object for editor modification
                    selectedSerializedAction = new SerializedObject(selectedNode);
                    selectedSerializedAction.Update();
                }

                selectedSerializedAction.Update();
                ActionTreeTask task = selectedNode as ActionTreeTask;
                currentProperties = task.getAllProperties(selectedSerializedAction);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(task.taskTypeName);
                EditorGUILayout.Space(10);
                foreach(SerializedProperty p in currentProperties)
                {
                    EditorGUILayout.PropertyField(p);
                }
                EditorGUILayout.EndVertical();
 
                selectedSerializedAction.ApplyModifiedProperties();
            }
            else
            {
                GUILayout.Label("Selected node isn't an action node");
            }
        }
        private void drawTreeArea()
        {
            //   scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true);
            Rect canvasRect = new Rect(0, 0, canvasSize * 2, canvasSize * 2);
            Texture2D bgTex = Resources.Load("background") as Texture2D;
            Rect coord = new Rect(0, 0, canvasSize / bgTextureSize, canvasSize / bgTextureSize);

            GUI.DrawTextureWithTexCoords(canvasRect, bgTex, coord);

            foreach (BehaviourTreeTask node in selectedBehaviour.GetNodes())
            {
                DrawConnections(node);
            }

            foreach (BehaviourTreeTask node in selectedBehaviour.GetNodes())
            {
                DrawNode(node);
            }

            //   GUILayout.EndScrollView();
        }
        private void drawTopPanel()
        {
            Color was = GUI.backgroundColor;
            GUI.backgroundColor = Color.grey;
            GUILayout.BeginHorizontal(GUILayout.Height(topPanelHeight));

            //inspector
            if (selectedInspector == InspectorType.INSPECTOR) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.grey;
            if (GUILayout.Button("Inspector", GUILayout.Width(100)))
            {
                selectedInspector = InspectorType.INSPECTOR;
            }

            //tasks
            if (selectedInspector == InspectorType.TASKS) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.grey;
            if (GUILayout.Button("Tasks", GUILayout.Width(100)))
            {
                selectedInspector = InspectorType.TASKS;
                //get all tasks
                getAllTasks();
            }

            //parameters
            if (selectedInspector == InspectorType.PARAMETERS) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.grey;
            if (GUILayout.Button("Parameters", GUILayout.Width(100)))
            {
                selectedInspector = InspectorType.PARAMETERS;
            }

            GUILayout.EndHorizontal();
            GUI.backgroundColor = was;

        }
        private void onDrawTasksPanel()
        {
            if (selectedNode == null)
            {
                GUILayout.Label("No selected node");
            }
            else if(selectedNode.taskType == TaskType.ACTION)
            {
                GUILayout.Label("Selected node is a leaf node");
            }
            else if(selectedNode.taskType == TaskType.DECORATOR && selectedNode.children.Count>0
                && ((DecoratorTreeTask)selectedNode).OnlySingleChildAllowed)
            {
                GUILayout.Label("Selected node has OnlyOneChildAllowed flag enabled, cannot add new childen");
            }
            else
            {
                GUILayout.BeginVertical();

                GUILayout.Space(20);
                GUILayout.Label("__________________COMPOSITES__________________");
                GUILayout.Space(20);
                foreach (BehaviourTreeTask current in behaviourTasks_Composite)
                {
                   // Debug.Log(current.name);
                    if (GUILayout.Button(current.taskTypeName))
                    {
                        Type toCreate = current.GetType();
                        Vector2 createAt = computeStartPositionFor(selectedNode);
                        var element = selectedBehaviour.createNodeOfType(toCreate, selectedNode, createAt);
                        if (element != null)
                        {
                            selectedNode = element;
                            selectedNode.editorTexture = current.editorTexture;
                            selectedNode.taskTypeName = current.taskTypeName;
                            selectedNode.taskType = current.taskType;
                        }
                    }
                }
                GUILayout.Space(20);
                GUILayout.Label("__________________DECORATORS__________________");
                GUILayout.Space(20);
                foreach (BehaviourTreeTask current in behaviourTasks_Decorators)
                {
                    // Debug.Log(current.name);
                    if (GUILayout.Button(current.taskTypeName))
                    {
                        Type toCreate = current.GetType();
                        Vector2 createAt = computeStartPositionFor(selectedNode);
                        var element = selectedBehaviour.createNodeOfType(toCreate, selectedNode, createAt);
                        if (element != null)
                        {
                            selectedNode = element;
                            selectedNode.editorTexture = current.editorTexture;
                            selectedNode.taskTypeName = current.taskTypeName;
                            selectedNode.taskType = current.taskType;
                        }
                    }
                }
                GUILayout.Space(20);
                GUILayout.Label("__________________ACTIONS__________________");
                GUILayout.Space(20);
                foreach (BehaviourTreeTask current in behaviourTasks_Actions)
                {
                    // Debug.Log(current.name);
                    if (GUILayout.Button(current.taskTypeName))
                    {
                        Type toCreate = current.GetType();
                        Vector2 createAt = computeStartPositionFor(selectedNode);
                        var element = selectedBehaviour.createNodeOfType(toCreate, selectedNode, createAt);
                        selectedNode = element;
                        selectedNode.editorTexture = current.editorTexture;
                        selectedNode.taskTypeName = current.taskTypeName;
                        selectedNode.taskType = current.taskType;
                    }
                }

                GUILayout.EndVertical();
            }
        }
        private void DrawConnections(BehaviourTreeTask node)
        {
            Vector2 startPosition = new Vector2(node.NodeRect.center.x, node.NodeRect.yMax - 10);
            if (!selectedBehaviour.hasAnyChildren(node)) return;
            foreach (BehaviourTreeTask child in selectedBehaviour.getAllChildren(node))
            {
                Vector2 endPosition = new Vector2(child.NodeRect.center.x, child.NodeRect.yMin + 10);
                Vector2 distance = endPosition - startPosition;
                distance.x = 0;
                distance.y *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + distance, endPosition - distance, Color.white, null, 3.0f);
            }
        }
        ///////////////////////////////////////////////////////
        #endregion

        private void processEvents()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && currentNode == null)
            {
                //Debug.Log("mouse pos scaled:  " + mousePosition + " mouse position normal: " + currentEvent.mousePosition+" scaling: "+scaling);
                currentNode = getNodeAt(currentEvent.mousePosition - new Vector2(leftPanelSize.x, topPanelHeight));
                if (currentNode != null)
                {
                    localOffsetForDrag = currentEvent.mousePosition;
                    Selection.activeObject = currentNode;

                    //set as selectd node
                    selectedNode = currentNode;
                    ActionTreeTask actionTask = selectedNode as ActionTreeTask;
                    if(actionTask!=null)
                    {
                        //create serialized object for editor modification
                        selectedSerializedAction = new SerializedObject(selectedNode);
                        selectedSerializedAction.Update();
                    }
               
                    GUI.changed = true;
                }
                else
                {
                    if (currentEvent.mousePosition.x > leftPanelSize.x && currentEvent.mousePosition.x < this.position.width- GUI.skin.verticalScrollbar.fixedWidth * 2.5f
                        && currentEvent.mousePosition.y < this.position.height-20.0f)
                    {
                        draggingCanvas = true;
                        draggingOffset = currentEvent.mousePosition + scrollPosition;
                        Selection.activeObject = selectedBehaviour;
                    }
                }
            }
            else if (currentEvent.type == EventType.MouseDrag && currentNode != null)
            {
                Undo.RecordObject(selectedBehaviour, "Drag panel");
                Rect oldRect = currentNode.NodeRect;
                Vector2 offset = (currentEvent.mousePosition - localOffsetForDrag)/scaling;
                localOffsetForDrag = currentEvent.mousePosition;
                oldRect.position += offset;
                oldRect.position = new Vector2(Mathf.Clamp(oldRect.position.x,0,10000),Mathf.Clamp(oldRect.position.y,0,10000));
                currentNode.NodeRect = oldRect;
                GUI.changed = true;
            }
            else if (currentEvent.type == EventType.MouseDrag && draggingCanvas)
            {
                Undo.RecordObject(selectedBehaviour, "Drag view");
                scrollPosition = draggingOffset - currentEvent.mousePosition;
                GUI.changed = true;
            }
            else if (currentEvent.type == EventType.MouseUp && currentNode != null)
            {
                currentNode = null;
            }
            else if (currentEvent.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        private BehaviourTreeTask getNodeAt(Vector2 pos)
        {
            BehaviourTreeTask selectedNode = null;
            pos += scrollPosition;
            pos /= scaling;
            foreach (BehaviourTreeTask current in selectedBehaviour.GetNodes())
            {
                Rect local = current.NodeRect;
                //local.width *= scaling;
               // local.height *= scaling;
                if (local.Contains(pos))
                {
                    selectedNode = current;
                }
            }
            return selectedNode;
        }

        private Vector2 computeStartPositionFor(BehaviourTreeTask forTask)
        {
            Rect startRect = forTask.NodeRect;
            Vector2 pos = startRect.position;
            //check children and put it at the most right position
            BehaviourTreeTask lastChild = forTask.getLastChildPos();
            if (lastChild == null)
            {
                pos.y += startRect.height * 2;
            }
            else
            {
                pos = lastChild.NodeRect.position;
                pos.x += startRect.width * 2;
            }
            return pos;
        }

        /// <summary>
        /// Callback when we change what is selected in editor
        /// When editor is open and we change selected behaviour tree we need to update selectedBehaviour and repaint window
        /// </summary>
        private void onSelectionChanged()
        {
            BehaviourTree behaviour = Selection.activeObject as BehaviourTree;
            if (behaviour != null)
            {
                selectedBehaviour = behaviour;
                Repaint();
            }
            else
            {
                GameObject obj = Selection.activeObject as GameObject;
                if(obj)
                {
                    BehaviourTreeController controler = obj.GetComponent<BehaviourTreeController>();
                    if(controler!=null)
                    {
                        selectedBehaviour = controler.source;
                        runtimeController = controler;
                    }
                }
            }
        }
    }
}
