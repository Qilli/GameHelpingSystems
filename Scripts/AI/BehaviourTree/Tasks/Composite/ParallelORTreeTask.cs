using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "ParallelORTreeTask", menuName = "ParallelOR", order = 51)]
    public class ParallelORTreeTask : CompositeTreeTask
    {

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent = null)
        {
            children.Sort();
            ParallelORTreeTaskRuntime rn = new ParallelORTreeTaskRuntime();
            rn.type = TaskType.COMPOSITE;
            rn.parent = parent;
            foreach (BehaviourTreeTask child in children)
            {
                rn.children.Add(child.getRuntimeTask(runtimeController,rn));
            }
            return rn;
        }
    }

    public class ParallelORTreeTaskRuntime: BehaviourTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            bool hasSuccess = false;
            bool hasRunning = false;
            foreach (BehaviourTreeTaskRuntime t in children)
            {
                TaskResult result = t.run(controller);
                if (result == TaskResult.SUCCESS) hasSuccess = true;
                else if (result == TaskResult.RUNNING) hasRunning = true;
            }
            if (hasRunning) lastResult = TaskResult.RUNNING;
            else if (hasSuccess) lastResult = TaskResult.SUCCESS;
            else lastResult = TaskResult.FAIL;
            return lastResult;
        }
    }

}
