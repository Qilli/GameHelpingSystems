using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    public class BehaviourTreesManager : Base.ObjectsControl.BaseObject
    {
        public class BehaviourTreeInstances
        {
            public List<BehaviourTreeController> behaviourTreeInstances= new List<BehaviourTreeController>();
            public BehaviourTree definition;

            public void removeTree(BehaviourTreeController ctrl)
            {
                behaviourTreeInstances.Remove(ctrl);
            }
        }

        protected List<BehaviourTreeInstances> btInstances = new List<BehaviourTreeInstances>();

        public virtual void registerBehaviourTree(BehaviourTreeController treeController)
        {
            foreach(BehaviourTreeInstances i in btInstances)
            {
                if(i.definition == treeController.source)
                {
                    i.behaviourTreeInstances.Add(treeController);
                    return;
                }
            }

            //first istance of this type
            BehaviourTreeInstances instance = new BehaviourTreeInstances();
            instance.definition = treeController.source;
            instance.behaviourTreeInstances.Add(treeController);
            btInstances.Add(instance);
        }

        public virtual void removeBehaviourTree(BehaviourTreeController treeController)
        {
            foreach(BehaviourTreeInstances instance in btInstances)
            {
                if(instance.definition == treeController.source)
                {
                    instance.removeTree(treeController);
                    return;
                }
            }
        }

        public override void onUpdate(float delta)
        {
            base.onUpdate(delta);
            //do a step on every active bt
            foreach(BehaviourTreeInstances i in btInstances)
            {
                foreach(BehaviourTreeController ctrl in i.behaviourTreeInstances)
                {
                    ctrl.stepTree(0.2f);
                }
            }

        }
    }
}
