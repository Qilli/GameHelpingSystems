using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "SequenceTreeTask", menuName = "Sequence", order = 51)]
    public class SequenceTreeTask : CompositeTreeTask
    {

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent = null)
        {
            SequenceTreeTaskRuntime rn = new SequenceTreeTaskRuntime();
            rn.type = TaskType.COMPOSITE;
            rn.parent = parent;
            foreach (BehaviourTreeTask child in children)
            {
                rn.children.Add(child.getRuntimeTask(runtimeController,rn));
            }
            return rn;
        }
    }

    public class SequenceTreeTaskRuntime: BehaviourTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            foreach (BehaviourTreeTaskRuntime t in children)
            {
                TaskResult result = t.run(controller);
                if (result != TaskResult.SUCCESS)
                {
                    lastResult = result;
                    return result;
                }
            }
            lastResult = TaskResult.SUCCESS;
            return TaskResult.SUCCESS;
        }
    }

}
