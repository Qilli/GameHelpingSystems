using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "BehaviourTree", menuName = "System/Design/BehaviourTree", order = 0)]
    public class BehaviourTree : ScriptableObject, ISerializationCallbackReceiver
    {
        //paramaters
        [SerializeField]
        private List<BehaviourTreeTask> nodes = new List<BehaviourTreeTask>();
        //Geters
        public IEnumerable<BehaviourTreeTask> GetNodes() { return nodes; }
        public BehaviourTreeTask getRootNode() { return nodes[0]; }
        //every BT needs a bilboard to keep shared variables
        [SerializeField]
        private Blackboard blackboard= new Blackboard();

#if UNITY_EDITOR
        private void Awake()
        {
        }
#endif
        private void OnValidate()
        {     

        }

        #region GAMEPLAY

        public Blackboard Blackboard
        {
            get
            {
                return blackboard;
            }
        }

        public virtual void init()
        {
        }

        #endregion



        /// <summary>
        /// generate runtime nodes from SO definitions
        /// </summary>
        public BehaviourTreeTaskRuntime generateRuntimeNodes(BehaviourTreeController runtimeControler)
        {
            return nodes[0].getRuntimeTask(runtimeControler,null);
        }

        /// <summary>
        /// Get nodes count
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public int getNodesCount() { return nodes.Count; }

        public IEnumerable<BehaviourTreeTask> getAllChildren(BehaviourTreeTask parentNode)
        {
            foreach(BehaviourTreeTask bt in parentNode.NodeChildren)
            {
                    yield return bt;
            }
        }

        ///node may have 0 children
        public bool hasAnyChildren(BehaviourTreeTask forNode)
        {
            return forNode.NodeChildren.Count > 0 ? true : false;
        }

#if UNITY_EDITOR


        /// <summary>
        /// add empty child for selected node
        /// </summary>
        /// <param name="createNode"></param>
        public void createNode(BehaviourTreeTask parent,Vector2 createAt)
        {
            BehaviourTreeTask newNode = MakeNode(parent, createAt);
            Undo.RegisterCreatedObjectUndo(newNode, "Undo Create Node");
            Undo.RecordObject(this, "Add new node");
            AddNode(newNode);
        }

        public BehaviourTreeTask createNodeOfType(Type t,BehaviourTreeTask parent, Vector2 createAt)
        {
            BehaviourTreeTask newNode = MakeNodeOfType(t, parent, createAt);
            newNode.name = System.Guid.NewGuid().ToString();
            Rect oldRect = newNode.NodeRect;
            newNode.NodeRect = new Rect(createAt.x, createAt.y, oldRect.width, oldRect.height);
            Undo.RegisterCreatedObjectUndo(newNode, "Undo Create Node");
            Undo.RecordObject(this, "Add new node");
            AddNode(newNode);
            return newNode;
        }

        public void createExternalNode(BehaviourTreeTask newNode,BehaviourTreeTask parent, Vector2 createAt)
        {
            newNode.name = System.Guid.NewGuid().ToString();
            Rect oldRect = newNode.NodeRect;
            newNode.NodeRect = new Rect(createAt.x, createAt.y, oldRect.width, oldRect.height);
            if (parent != null)
            {
                parent.addNewChild(newNode);
            }
            Undo.RegisterCreatedObjectUndo(newNode, "Undo Create Node");
            Undo.RecordObject(this, "Add new node");
            AddNode(newNode);
        }

        private void AddNode(BehaviourTreeTask newNode)
        {
            nodes.Add(newNode);
            newNode.treeOwner = this;
            OnValidate();
        }

        private static BehaviourTreeTask MakeNode(BehaviourTreeTask parent, Vector2 createAt)
        {
            BehaviourTreeTask newNode = CreateInstance<BehaviourTreeTask>();
            newNode.name = System.Guid.NewGuid().ToString();
            Rect oldRect = newNode.NodeRect;
            newNode.NodeRect = new Rect(createAt.x, createAt.y, oldRect.width, oldRect.height);
            if (parent != null)
            {
                parent.addNewChild(newNode);
            }

            return newNode;
        }

        private static BehaviourTreeTask MakeNodeOfType(Type t,BehaviourTreeTask parent, Vector2 createAt)
        {
            BehaviourTreeTask newNode = CreateInstance(t.Name) as BehaviourTreeTask;
            newNode.name = System.Guid.NewGuid().ToString();
            Rect oldRect = newNode.NodeRect;
            newNode.NodeRect = new Rect(createAt.x, createAt.y, oldRect.width, oldRect.height);
            if (parent != null)
            {
                parent.addNewChild(newNode);
            }

            return newNode;
        }

        /// <summary>
        /// remove node and all his connections
        /// </summary>
        /// <param name="node"></param>
        public void removeNode(BehaviourTreeTask node)
        {
            List<BehaviourTreeTask> childrenTemp = new List<BehaviourTreeTask>(node.children);
            foreach(BehaviourTreeTask t in childrenTemp)
            {
                removeNode(t);
            }     
            nodes.Remove(node);      
            //remove all connections
            foreach(BehaviourTreeTask n in nodes)
            {
                n.removeChildConnection(node);
            }          
            OnValidate();
            Undo.DestroyObjectImmediate(node);
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                Texture2D root = Resources.Load("root") as Texture2D;
                BehaviourTreeTask node = CreateInstance<RootTreeTask>();
                node.editorTexture = root;
                node.taskTypeName = "ROOT";
                node.taskType = TaskType.ROOT;
                node.name = System.Guid.NewGuid().ToString();
                Rect oldRect = node.NodeRect;
                node.NodeRect = new Rect(100,100, oldRect.width, oldRect.height);
                AddNode(node);

            }
            if (AssetDatabase.GetAssetPath(this)!="")
            {
                foreach(BehaviourTreeTask n in nodes)
                {
                    if(AssetDatabase.GetAssetPath(n)=="")
                    {
                        AssetDatabase.AddObjectToAsset(n,this);
                    }
                }

            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }

    }
}
