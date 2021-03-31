using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    public class BehaviourTreeController : Base.ObjectsControl.BaseObject
    {
        public class TreeStatus
        {
            public TaskResult lastResult= TaskResult.SUCCESS;
            public List<TaskResult> executeStack = new List<TaskResult>();
            public bool fullTreeTraversalOnRunning = false;

            public void clearCacheResult()
            {
                for(int a=0;a<executeStack.Count;++a)
                {
                    executeStack[a] = TaskResult.NONE;
                }
            }
        }

        public class ActionElement
        {
            public ActionTreeTask task = null;
            public int id;
        }

        public BehaviourTree source;
        public AI.Agents.AIAgent agent;

        protected List<ActionElement> actionsList = new List<ActionElement>();
        protected TreeStatus currentStatusIterator = new TreeStatus();

        public TreeStatus getTreeIterator()
        {
            return currentStatusIterator;
        }

        public override void init()
        {
            base.init();
            if(source == null)
            {
                Base.Log.Logging.Log("Source for behaviour tree is null, cannot create bt controller", Log.BaseLogType.ERROR);
                return;
            }
            else
            {

                //init controller
                GlobalDataContainer.It.behaviourTreesMgr.registerBehaviourTree(this);
                currentStatusIterator.lastResult = TaskResult.SUCCESS;
                source.getAllActions(actionsList);
                for(int a=0;a<source.getNodesCount();++a)
                {
                    currentStatusIterator.executeStack.Add(TaskResult.NONE);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (GlobalDataContainer.It!=null&&GlobalDataContainer.It.behaviourTreesMgr != null)
            {
                GlobalDataContainer.It.behaviourTreesMgr.registerBehaviourTree(this);
            }
        }

        public virtual void stepTree(float timeStep)
        {
            TaskResult lastResult = source.stepTree(this, 0.2f);
        }
    }
}
