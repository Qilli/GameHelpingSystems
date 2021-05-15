using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "SelectorTreeTask", menuName = "Selector", order = 51)]
    public class SelectorTreeTask : CompositeTreeTask
    {
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent=null)
        {
            children.Sort();
            SelectorTreeTaskRuntime rn = new SelectorTreeTaskRuntime();
            rn.type = TaskType.COMPOSITE;
            rn.parent = parent;
            foreach (BehaviourTreeTask child in children)
            {
                rn.children.Add(child.getRuntimeTask(runtimeController,rn));
            }
            return rn;
        }
    }

    public class SelectorTreeTaskRuntime: BehaviourTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            foreach (BehaviourTreeTaskRuntime t in children)
            {
                TaskResult result = t.run(controller);
                if (result != TaskResult.FAIL)
                {
                    lastResult = result;
                    return result;
                }
            }
            lastResult = TaskResult.FAIL;
            return TaskResult.FAIL;
        }
    }

}
