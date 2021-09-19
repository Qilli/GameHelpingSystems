using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Base.AI.Behaviours
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StopNavMovementTask", order = 1)]
    public class StopNavMovementTask : ActionTreeTask
    {
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent = null)
        {
            StopNavMovementTasRuntime rn = new StopNavMovementTasRuntime();
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            return list;
        }

#endif
    }

    public class StopNavMovementTasRuntime : ActionTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            //stop nav
            controller.agent.NavAgent.isStopped = true;
            //we need to look at our target, force it on instant
            controller.agent.AgentKinematics.velocity = Vector3.zero;
            controller.agent.AgentKinematics.rotation = 0;
            //cache result
            return lastResult = TaskResult.SUCCESS;
        }

    }
}
