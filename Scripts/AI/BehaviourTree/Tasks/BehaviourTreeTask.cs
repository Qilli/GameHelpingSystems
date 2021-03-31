using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    public enum TaskResult
    {
        SUCCESS,
        FAIL,
        RUNNING,
        NONE
    }

    public enum TaskType
    {
        ACTION,
        CONDITION,
        COMPOSITE,
        DECORATOR,
        ROOT
    }

    public class BehaviourTreeTask : ScriptableObject
    {
        //for bt editor
        [HideInInspector]
        private int index;
        public TaskType taskType;  
        public List<BehaviourTreeTask> children = new List<BehaviourTreeTask>();
        public BehaviourTreeTask parent = null;
        public bool useAsTaskSourceEditor = false;

        public virtual TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            return TaskResult.SUCCESS;
        }

        public int TaskIndex
        {
            set
            {
                index = value;
            }
            get { return index; }
        }

        protected bool hasReadyResult(BehaviourTreeController.TreeStatus controller)
        {
            return controller.executeStack[TaskIndex] != TaskResult.NONE && controller.executeStack[TaskIndex] != TaskResult.RUNNING && controller.fullTreeTraversalOnRunning==false ;
        }

        //EDITOR THINGS
        [Header("EDITOR")]
        public Texture2D editorTexture;
        public string taskTypeName;
        [SerializeField]
        private string description;
        [SerializeField]
        private Rect rect = new Rect(20, 20, 100, 100);

        //node description
        public string NodeDescription
        {
            get { return description; }
            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Change Node's Description");
#endif
                description = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif  
            }
        }

        //Change and get node rect
        public Rect NodeRect
        {
            get
            {
                return rect;
            }

            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Change Node's Rect");
#endif
                rect = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif 
            }
        }

        //get and modify children
        public List<BehaviourTreeTask> NodeChildren
        {
            get
            {
                return children;
            }
        }

        public bool isChildOf(BehaviourTreeTask linkingNode)
        {
            return linkingNode.children.Contains(this);
        }

#if UNITY_EDITOR

        public void removeChildConnection(BehaviourTreeTask child)
        {
            Undo.RecordObject(this, "Remove a child");
            children.Remove(child);
            EditorUtility.SetDirty(this);
        }

        public void addNewChild(BehaviourTreeTask child)
        {
            Undo.RecordObject(this, "Add a child");
            child.parent = this;
            children.Add(child);
            EditorUtility.SetDirty(this);
        }

        public BehaviourTreeTask getLastChildPos()
        {
            if (children.Count == 0) return null;
            float max = -1000000;
            BehaviourTreeTask current = null;
            foreach (BehaviourTreeTask t in children)
            {
                if (t.NodeRect.position.x > max)
                {
                    current = t;
                    max = t.NodeRect.position.x;
                }
            }
            return current;
        }
#endif



    }


}
