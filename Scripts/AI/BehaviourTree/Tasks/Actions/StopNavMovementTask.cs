using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Base.AI.Behaviours
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StopNavMovementTask", order = 1)]
    public class StopNavMovementTask : ActionTreeTask
    {
        public EditorSharedVariable useNavMeshMovement =
        new EditorSharedVariable
        {
            name = "",
            type = SharedVariable.SharedType.BOOL
        };
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController, BehaviourTreeTaskRuntime parent = null)
        {
            StopNavMovementTasRuntime rn = new StopNavMovementTasRuntime();
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            rn.useNavMeshMovement = runtimeController.Blackboard.getVariableByName(useNavMeshMovement.name) as SharedBool;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("useNavMeshMovement"));
            return list;
        }

#endif
    }

    public class StopNavMovementTasRuntime : ActionTreeTaskRuntime
    {
        public SharedBool useNavMeshMovement;
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            if (useNavMeshMovement.value)
            {
                //stop nav
                controller.agent.NavAgent.isStopped = true;
                //we need to look at our target, force it on instant
                controller.agent.AgentKinematics.Velocity = Vector3.zero;
                controller.agent.AgentKinematics.RotationHorizontal = 0;
            }
            //cache result
            return lastResult = TaskResult.SUCCESS;
        }

    }
}
