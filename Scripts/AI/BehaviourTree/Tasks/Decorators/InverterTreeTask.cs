using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "InverterTreeTask", menuName = "Inverter", order = 51)]
    public class InverterTreeTask : DecoratorTreeTask
    {
        public InverterTreeTask():base(true)
        {

        }
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent=null)
        {
            children.Sort();
            InverterTreeTaskRuntime rn = new InverterTreeTaskRuntime();
            rn.type = TaskType.COMPOSITE;
            rn.parent = parent;
            foreach (BehaviourTreeTask child in children)
            {
                rn.children.Add(child.getRuntimeTask(runtimeController,rn));
            }
            return rn;
        }
    }

    public class InverterTreeTaskRuntime: BehaviourTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            foreach (BehaviourTreeTaskRuntime t in children)
            {
                TaskResult result = t.run(controller);
                if (result == TaskResult.SUCCESS) return lastResult = TaskResult.FAIL;
                else if (result == TaskResult.FAIL) return lastResult = TaskResult.SUCCESS;
                else return lastResult = TaskResult.RUNNING;
            }
            lastResult = TaskResult.FAIL;
            return TaskResult.FAIL;
        }
    }

}
