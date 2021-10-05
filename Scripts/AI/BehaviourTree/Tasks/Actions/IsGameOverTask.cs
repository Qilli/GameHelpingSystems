using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IsGameOverTask", order = 1)]
    public class IsGameOverTask : ActionTreeTask
    {

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            IsGameOverTaskRuntime rn = new IsGameOverTaskRuntime();
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

    public class IsGameOverTaskRuntime : ActionTreeTaskRuntime
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            if (Base.AI.Behaviours.GlobalBlackboard.It.getBooleanParamWithName("IsGameOver"))
            {
                return lastResult = TaskResult.SUCCESS;
            }
            //cache result
            return lastResult = TaskResult.FAIL;
        }
    }

}
