using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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

    public class BehaviourTreeTask : ScriptableObject, System.IComparable<BehaviourTreeTask>
    {
        //gameplay data
        public BehaviourTree treeOwner;
        public TaskType taskType;  
        public List<BehaviourTreeTask> children = new List<BehaviourTreeTask>();
        public BehaviourTreeTask parent = null;
        public bool useAsTaskSourceEditor = false;


        //EDITOR THINGS
        [Header("EDITOR")]
        public Texture2D editorTexture;
        public string taskTypeName;
        [SerializeField]
        private string description;
        [SerializeField]
        private Rect rect = new Rect(20, 20, 100, 100);

        public virtual BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent = null)
        {
            BehaviourTreeTaskRuntime rn = new BehaviourTreeTaskRuntime();
            rn.type = TaskType.ROOT;
            return rn;
        }

        public int CompareTo(BehaviourTreeTask other)
        {
            if (this.rect.min.x < other.rect.min.x) return -1;
            else return 1;
        }

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

    public class BehaviourTreeTaskRuntime
    {
        public TaskResult lastResult;
        public TaskType type;
        public List<BehaviourTreeTaskRuntime> children = new List<BehaviourTreeTaskRuntime>();
        public BehaviourTreeTaskRuntime parent = null;

        public virtual TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            return TaskResult.SUCCESS;
        }

        public virtual void setLastResult(TaskResult newResult, bool clearChildren=false)
        {
            lastResult = newResult;
            if(clearChildren)
            {
                foreach(BehaviourTreeTaskRuntime child in children)
                {
                    child.setLastResult(newResult,clearChildren);
                }
            }
        }

        protected bool hasReadyResult(BehaviourTreeController.TreeStatus controller)
        {
            return lastResult != TaskResult.NONE && lastResult != TaskResult.RUNNING && controller.fullTreeTraversalOnRunning == false;
        }
    }


}
