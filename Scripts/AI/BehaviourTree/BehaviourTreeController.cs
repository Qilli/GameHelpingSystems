using System;
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
            public bool fullTreeTraversalOnRunning = false;
        }

        public class ActionElement
        {
            public ActionTreeTask task = null;
            public int id;
        }

        public BehaviourTree source;
        public AI.Agents.AIAgent agent;

        protected BehaviourTreeTaskRuntime rootNode;
        protected TreeStatus currentStatusIterator = new TreeStatus();
        protected Blackboard blackboard;

        public TreeStatus getTreeIterator()
        {
            return currentStatusIterator;
        }

        public Blackboard Blackboard
        {
            get
            {
                return blackboard;
            }
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
                blackboard = source.Blackboard.getCopy();
                rootNode=source.generateRuntimeNodes(this);
                clearLastResults();
            }
        }

        public virtual void stepTree(float timeStep)
        {
            //take first element from the stack
            BehaviourTreeController.TreeStatus status = getTreeIterator();

            if (status.lastResult == TaskResult.RUNNING && status.fullTreeTraversalOnRunning == false)
            {
                //dont clear cache and step through tree
                status.lastResult = rootNode.run(getTreeIterator());     
            }
            else
            {
                //clear cache result
                clearLastResults();
                //do a tree traversal
                status.lastResult = rootNode.run(getTreeIterator());
            }
        }

        private void clearLastResults()
        {
            rootNode.setLastResult(TaskResult.NONE, true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (GlobalDataContainer.It != null && GlobalDataContainer.It.behaviourTreesMgr != null)
            {
                GlobalDataContainer.It.behaviourTreesMgr.registerBehaviourTree(this);
            }
        }

    }
}
