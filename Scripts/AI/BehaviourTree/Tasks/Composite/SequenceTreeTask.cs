using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "SequenceTreeTask", menuName = "Sequence", order = 51)]
    public class SequenceTreeTask : CompositeTreeTask
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return controller.executeStack[TaskIndex];
            foreach (BehaviourTreeTask t in children)
            {
                TaskResult result = t.run(controller);
                if (result != TaskResult.SUCCESS)
                {
                    controller.executeStack[TaskIndex] = result;
                    return result;
                }
            }
            controller.executeStack[TaskIndex] = TaskResult.SUCCESS;
            return TaskResult.SUCCESS;
        }
    }

}
