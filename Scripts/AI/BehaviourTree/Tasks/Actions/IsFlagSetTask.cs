using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
   // [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IsFlagSetTask", order = 1)]
    public class IsFlagSetTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable flag= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.BOOL};
        public bool resetFlag = false;
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            IsFlagSetTaskRuntime rn = new IsFlagSetTaskRuntime();
            rn.flag = runtimeController.Blackboard.getVariableByName(flag.name) as SharedBool;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //params
            rn.resetFlag = resetFlag;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("flag"));
            list.Add(obj.FindProperty("resetFlag"));
            return list;
        }

#endif
    }

    public class IsFlagSetTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedBool flag;
        public bool resetFlag = false;

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if(flag.value)
            {
                if(resetFlag)
                {
                    flag.value = false;
                }
                return lastResult = TaskResult.SUCCESS;
            }
  
            //cache result
            return lastResult = TaskResult.FAIL;
        }
    }

}
