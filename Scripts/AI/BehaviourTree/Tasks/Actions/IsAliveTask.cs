using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DealDamageTargetTask", order = 1)]
    public class IsAliveTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable target= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.TRANSFORM};
        public bool yourself = false;
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            IsAliveTaskRuntime rn = new IsAliveTaskRuntime();
            rn.target = runtimeController.Blackboard.getVariableByName(target.name) as SharedTransform;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.yourself = yourself;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("target"));
            list.Add(obj.FindProperty("yourself"));
            return list;
        }

#endif
    }

    public class IsAliveTaskRuntime: ActionTreeTaskRuntime
    {
        public SharedTransform target;
        public bool yourself;
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            if (target.value != null && yourself==false)
            {
                Base.Game.IIsAlive control = target.value.GetComponent<Base.Game.IIsAlive>();
                if(control.isAlive())
                {
                    return lastResult = TaskResult.SUCCESS;
                }
                return lastResult = TaskResult.FAIL;
            }
            else if(yourself)
            {
                Base.Game.IIsAlive control = controller.agent.GetComponent<Base.Game.IIsAlive>();
                if (control.isAlive())
                {
                    return lastResult = TaskResult.SUCCESS;
                }
                return lastResult = TaskResult.FAIL;
            }
  
            //cache result
            return lastResult = TaskResult.FAIL;
        }
    }

}
