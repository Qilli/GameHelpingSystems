using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
   // [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    public class IsCloseEnoughTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable  distance= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.FLOAT };
        public EditorSharedVariable target= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.TRANSFORM};
        public bool useXAxis = false;
        public bool useYAxis = false;
        public bool useZAxis = false;
        public bool resetAgentVelocity = true;

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            IsCloseEnoughTaskRuntime rn = new IsCloseEnoughTaskRuntime();
            rn.target = runtimeController.Blackboard.getVariableByName(target.name) as SharedTransform;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.distance = runtimeController.Blackboard.getVariableByName(distance.name) as SharedFloat;
            rn.useXAxis = useXAxis;
            rn.useYAxis = useYAxis;
            rn.useZAxis = useZAxis;
            rn.resetAgentVelocity = resetAgentVelocity;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("distance"));
            list.Add(obj.FindProperty("target"));
            list.Add(obj.FindProperty("useXAxis"));
            list.Add(obj.FindProperty("useYAxis"));
            list.Add(obj.FindProperty("useZAxis"));
            list.Add(obj.FindProperty("resetAgentVelocity"));
            return list;
        }

#endif
    }

    public class IsCloseEnoughTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedTransform target;
        public SharedFloat distance;
        public bool useXAxis = false;
        public bool useYAxis = false;
        public bool useZAxis = false;
        public bool resetAgentVelocity = true;

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if (target.value == null)
            {
                return lastResult = TaskResult.FAIL;
            }
            Vector3 dir = new Vector3(useXAxis?target.value.position.x-controller.agent.AgentKinematics.Position.x:0,
                useYAxis ? target.value.position.y - controller.agent.AgentKinematics.Position.y : 0,
                useZAxis ? target.value.position.z - controller.agent.AgentKinematics.Position.z : 0);

            if(dir.magnitude <= distance.value)
            {
                if(resetAgentVelocity)
                {
                    controller.agent.AgentKinematics.Velocity = Vector3.zero;
                }
                return lastResult = TaskResult.SUCCESS;
            }
          
            //cache result
            return lastResult = TaskResult.FAIL;
        }

    }

}
