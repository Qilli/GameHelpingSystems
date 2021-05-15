using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
  // [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AttackTargetTask", order = 1)]
    public class AttackTargetTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable target = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.TRANSFORM };
        public EditorSharedVariable eventType = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.OBJECT };
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            AttackTargetTaskRuntime rn = new AttackTargetTaskRuntime();
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.target = runtimeController.Blackboard.getVariableByName(target.name) as SharedTransform;
            rn.eventType = runtimeController.Blackboard.getVariableByName(eventType.name) as SharedObject;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("target"));
            list.Add(obj.FindProperty("eventType"));
            return list;
        }

#endif
    }

    public class AttackTargetTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedTransform target;
        public SharedObject eventType;

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if(target.value==null)
            {
                return lastResult = TaskResult.FAIL;
            }

            //attack event
            Base.Events.GameEventID eventID = eventType.value as Base.Events.GameEventID;
            if(eventID==null)
            {
                return lastResult = TaskResult.FAIL;
            }

            //call event
            
           // GlobalDataContainer.It.eventsManager.dispatchEvent()

            //cache result
            return lastResult = TaskResult.SUCCESS;
        }

    }

}
