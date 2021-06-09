using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
  // [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    public class TimerTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable targetTimer = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.FLOAT };
        public bool repeatTimer = false;
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            TimerTaskRuntime rn = new TimerTaskRuntime();
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.targetTimer = runtimeController.Blackboard.getVariableByName(targetTimer.name) as SharedFloat;
            rn.repeatTimer = repeatTimer;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("targetTimer"));
            list.Add(obj.FindProperty("repeatTimer"));
            return list;
        }

#endif
    }

    public class TimerTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedFloat targetTimer;
        public float timer=0;
        public bool repeatTimer = false;
        public int counter = 0;

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if(counter>0 && !repeatTimer)
            {
                return lastResult = TaskResult.FAIL;
            }

            timer += Time.deltaTime;
            if(timer>targetTimer.value)
            {
                    timer = 0;
                    counter++;
                    return lastResult = TaskResult.SUCCESS;
            }

            //cache result
            return lastResult = TaskResult.FAIL;
        }

    }

}
