using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    public class BehaviourTreeController : Base.ObjectsControl.BaseObject,Base.Game.IOnLifeChangeActions
    {
        [System.Serializable]
        public class TreeStatus
        {
            public TaskResult lastResult= TaskResult.SUCCESS;
            public bool fullTreeTraversalOnRunning = false;
            public AI.Agents.AIAgent agent;
        }
        public class ActionElement
        {
            public ActionTreeTask task = null;
            public int id;
        }

        public BehaviourTree source;
        public AI.Agents.AIAgent agent;

        protected BehaviourTreeTaskRuntime rootNode;
        public TreeStatus currentStatusIterator = new TreeStatus();
        protected Blackboard blackboard;
        private Base.AI.Agents.IAgentParameters agentParametersControl;

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
            if (!inited)
            {
                base.init();
                if (source == null)
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
                    rootNode = source.generateRuntimeNodes(this);
                    clearLastResults();
                    currentStatusIterator.agent = agent;
                    //set base parameters
                    agentParametersControl = GetComponent<AI.Agents.IAgentParameters>();
                    agentParametersControl?.init();
                }
                inited = true;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            init();
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
           // onPreDestroy();
        }

        public void onPreDestroy()
        {
            if (GlobalDataContainer.It != null && GlobalDataContainer.It.behaviourTreesMgr != null)
            {
                GlobalDataContainer.It.behaviourTreesMgr.removeBehaviourTree(this);
            }
        }
        public void onBorn()
        {

        }
    }
}
