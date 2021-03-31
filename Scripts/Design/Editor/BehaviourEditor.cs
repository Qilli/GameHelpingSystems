using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


namespace Base.AI.Behaviours.Editor
{
    public class BehaviourEditor : EditorWindow
    {
        [NonSerialized]
        private BehaviourTree selectedBehaviour = null;
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
        private InspectorType selectedInspector = InspectorType.INSPECTOR;
        private List<BehaviourTreeTask> behaviourTasks = new List<BehaviourTreeTask>();
        private List<CompositeTreeTask> behaviourTasks_Composite = new List<CompositeTreeTask>();
        private List<ActionTreeTask> behaviourTasks_Actions = new List<ActionTreeTask>();

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

            //get all tasks
            getAllTasks();
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
            List<CompositeTreeTask> allData = Base.CommonCode.Common.FindAssetsByType<CompositeTreeTask>();
            behaviourTasks_Composite.Clear();
            behaviourTasks_Actions.Clear();

            foreach (CompositeTreeTask t in allData)
            {
                if (t.useAsTaskSourceEditor)
                {
                    behaviourTasks_Composite.Add(t);
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

            // Debug.Log($"found: {behaviourTasks_Composite.Count} tasks");
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
                    //selectedBehaviour.deleteNode(node);
                    nodeToRemove = node;
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
            }

            GUILayout.EndVertical();

            GUILayout.EndScrollView();

        }

        private void onDrawInspectorPanel()
        {
            if (selectedNode == null) return;
            if (selectedNode.taskType == TaskType.ACTION)
            {
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
                        selectedNode = element;
                        selectedNode.editorTexture = current.editorTexture;
                        selectedNode.taskTypeName = current.taskTypeName;
                        selectedNode.taskType = current.taskType;
                    }
                }
                GUILayout.Space(20);
                GUILayout.Label("__________________CONDITIONS__________________");
                GUILayout.Space(20);
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
        }
    }
}
