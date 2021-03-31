using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "SelectorTreeTask", menuName = "Selector", order = 51)]
    public class SelectorTreeTask : CompositeTreeTask
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return controller.executeStack[TaskIndex];
            foreach (BehaviourTreeTask t in children)
            {
                TaskResult result = t.run(controller);
                if (result != TaskResult.FAIL)
                {
                    controller.executeStack[TaskIndex] = result;
                    return result;
                }
            }
            controller.executeStack[TaskIndex] = TaskResult.FAIL;
            return TaskResult.FAIL;
        }
    }

}
