using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    public class MoveToTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable moveSpeed = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.FLOAT };
        public EditorSharedVariable target= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.TRANSFORM};
        public float targetRadius = 1.0f;

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            MoveToTaskRuntime rn = new MoveToTaskRuntime();
            rn.target = runtimeController.Blackboard.getVariableByName(target.name) as SharedTransform;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.moveSpeed = runtimeController.Blackboard.getVariableByName(moveSpeed.name) as SharedFloat;
            rn.targetRadius = targetRadius;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("moveSpeed"));
            list.Add(obj.FindProperty("target"));
            list.Add(obj.FindProperty("targetRadius"));
            return list;
        }

#endif
    }

    public class MoveToTaskRuntime: ActionTreeTaskRuntime
    {
        public SharedTransform target;
        public SharedFloat moveSpeed;
        public float targetRadius;

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if (target.value == null)
            {
                return lastResult = TaskResult.FAIL;
            }

            Transform targetPos = target.value;
            float speed = moveSpeed.value;

            //target direction
            Vector3 dir = new Vector3(targetPos.position.x - controller.agent.AgentKinematics.position.x, 0, targetPos.position.z-controller.agent.AgentKinematics.position.z);
            //check if are within a radius
            if (dir.magnitude<=targetRadius)
            {
                //already on place
                controller.agent.AgentKinematics.velocity = Vector3.zero;
                controller.agent.AgentKinematics.rotation = 0;
                return lastResult = TaskResult.SUCCESS;
            }

            //still need to move
            Vector3 velocity = dir.normalized * speed;
            Debug.DrawLine(controller.agent.AgentKinematics.position,
            controller.agent.AgentKinematics.position+velocity,Color.red,0.1f);
            //we need to look at our target, force it on instant
            controller.agent.AgentKinematics.velocity = velocity;
            controller.agent.AgentKinematics.orientation = Base.AI.Agents.KinematicData.DirectionToOrientation(dir.normalized); 
            controller.agent.AgentKinematics.rotation = 0;
            //cache result
            return lastResult = TaskResult.RUNNING;
        }

    }

}