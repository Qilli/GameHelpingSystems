using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
  // [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DealDamageTargetTask", order = 1)]
    public class DealDamageTask : ActionTreeTask
    {
        //params
        public EditorSharedVariable target = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.TRANSFORM };
        public EditorSharedVariable eventType = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.OBJECT };
        public EditorSharedVariable damageValue = new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.FLOAT };
        public bool dealToYourself = false;
        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            DealDamageTaskRuntime rn = new DealDamageTaskRuntime();
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.target = runtimeController.Blackboard.getVariableByName(target.name) as SharedTransform;
            rn.eventType = runtimeController.Blackboard.getVariableByName(eventType.name) as SharedObject;
            rn.damageValue = runtimeController.Blackboard.getVariableByName(damageValue.name) as SharedFloat;
            rn.dealToYourself = dealToYourself;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("target"));
            list.Add(obj.FindProperty("eventType"));
            list.Add(obj.FindProperty("damageValue"));
            list.Add(obj.FindProperty("dealToYourself"));
            return list;
        }

#endif
    }

    public class DealDamageTaskRuntime : ActionTreeTaskRuntime
    {
        public SharedTransform target;
        public SharedObject eventType;
        public SharedFloat damageValue;
        public bool dealToYourself;
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
            Base.Events.DealDamageEvent dealEvent = new Events.DealDamageEvent(eventID);
            if(dealToYourself)
            {
                dealEvent.damageTarget = controller.agent.transform;
            }
            else  dealEvent.damageTarget = target.value;
            Base.Game.DamageSourceInfo damagesource = new Game.DamageSourceInfo();
            damagesource.damageSources.Add(new Base.Game.DamageSourceInfo.SourceValue(Base.Game.DamageSourceInfo.DamageSource.DEFAULT, 100));

            dealEvent.damageValue = damagesource;
            GlobalDataContainer.It.eventsManager.dispatchEvent(dealEvent);

            //cache result
            return lastResult = TaskResult.SUCCESS;
        }

    }

}
