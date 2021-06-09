using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "RootTreeTask", menuName = "Root", order = 51)]
    public class RootTreeTask : BehaviourTreeTask
    {
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent=null)
        {
            RootTreeTaskRuntime runtime = new RootTreeTaskRuntime();
            runtime.type = TaskType.ROOT;

            foreach(BehaviourTreeTask child in children)
            {
                runtime.children.Add(child.getRuntimeTask(runtimeController,runtime));
            }
            return runtime;
        }
    }

    public class RootTreeTaskRuntime: BehaviourTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            return children[0].run(controller);
        }
    }
}
