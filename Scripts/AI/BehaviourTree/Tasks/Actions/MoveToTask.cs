using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEditor;
using UnityEngine;

namespace Base.AI.Behaviours
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    public class MoveToTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable
            moveSpeed =
                new EditorSharedVariable()
                { name = "", type = SharedVariable.SharedType.FLOAT };

        public EditorSharedVariable
            target =
                new EditorSharedVariable()
                { name = "", type = SharedVariable.SharedType.TRANSFORM };

        public float targetRadius = 1.0f;

        public EditorSharedVariable
            useNavMeshMovement =
                new EditorSharedVariable {
                    name = "",
                    type = SharedVariable.SharedType.BOOL
                };

        public override BehaviourTreeTaskRuntime
        getRuntimeTask(
            BehaviourTreeController runtimeController,
            BehaviourTreeTaskRuntime parent = null
        )
        {
            MoveToTaskRuntime rn = new MoveToTaskRuntime();
            rn.target =
                runtimeController.Blackboard.getVariableByName(target.name) as
                SharedTransform;
            rn.type = TaskType.ACTION;
            rn.parent = parent;

            //parameters
            rn.moveSpeed =
                runtimeController
                    .Blackboard
                    .getVariableByName(moveSpeed.name) as
                SharedFloat;
            rn.targetRadius = targetRadius;
            rn.useNavMeshMovement =
                runtimeController
                    .Blackboard
                    .getVariableByName(useNavMeshMovement.name) as
                SharedBool;
            return rn;
        }


#if UNITY_EDITOR

        public override List<SerializedProperty>
        getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("moveSpeed"));
            list.Add(obj.FindProperty("target"));
            list.Add(obj.FindProperty("targetRadius"));
            list.Add(obj.FindProperty("useNavMeshMovement"));
            return list;
        }


#endif
    }

    public class MoveToTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedTransform target;

        public SharedFloat moveSpeed;

        public float targetRadius;

        public SharedBool useNavMeshMovement;

        private Vector3
            lastTarget =
                new Vector3(float.PositiveInfinity,
                    float.PositiveInfinity,
                    float.PositiveInfinity);

        public override TaskResult
        run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if (target.value == null)
            {
                return lastResult = TaskResult.FAIL;
            }

            Transform targetPos = target.value;
            float speed = moveSpeed.value;
            if (useNavMeshMovement.value)
            {
                if (Vector3.Distance(targetPos.position, lastTarget) < 0.1f)
                {
                    //target did not change
                    controller.agent.NavAgent.isStopped = false;
                    controller.agent.NavAgent.updateRotation=true;
                }
                else
                {
                    //new target
                    lastTarget = targetPos.position;
                    controller.agent.NavAgent.isStopped = false;
                    controller.agent.NavAgent.destination = lastTarget;
                    controller.agent.NavAgent.updateRotation=true;
                    controller.agent.NavAgent.speed = speed;
                }
            }
            else
            {
                //target direction
                Vector3 dir =
                    new Vector3(targetPos.position.x -
                        controller.agent.AgentKinematics.Position.x,
                        0,
                        targetPos.position.z -
                        controller.agent.AgentKinematics.Position.z);

                //check if are within a radius
                if (dir.magnitude <= targetRadius)
                {
                    //already on place
                    controller.agent.AgentKinematics.Velocity = Vector3.zero;
                    controller.agent.AgentKinematics.RotationHorizontal = 0;
                    return lastResult = TaskResult.SUCCESS;
                }

                //still need to move
                Vector3 velocity = dir.normalized * speed;
                Debug
                    .DrawLine(controller.agent.AgentKinematics.Position,
                    controller.agent.AgentKinematics.Position + velocity,
                    Color.red,
                    0.1f);

                //we need to look at our target, force it on instant
                controller.agent.AgentKinematics.Velocity = velocity;
                controller.agent.AgentKinematics.OrientationHorizontal = Base.Physics.KinematicAgentData.DirectionToOrientation(dir.normalized);
                controller.agent.AgentKinematics.RotationHorizontal = 0;
            }

            //cache result
            return lastResult = TaskResult.RUNNING;
        }
    }
}
