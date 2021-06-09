using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
   // [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    public class SetMoveParamsTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable targetVelocity = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.VECTOR };

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            SetMoveParamTaskRuntime rn = new SetMoveParamTaskRuntime();
            rn.targetVelocity = runtimeController.Blackboard.getVariableByName(targetVelocity.name) as SharedVector;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("targetVelocity"));
            return list;
        }

#endif
    }

    public class SetMoveParamTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedVector targetVelocity;

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            //we need to look at our target, force it on instant
            controller.agent.AgentKinematics.velocity = targetVelocity.value;
            //cache result
            return lastResult = TaskResult.SUCCESS;
        }

    }

}
