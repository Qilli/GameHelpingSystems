using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    /// <summary>
    /// Simple log task
    /// </summary>
    public class LogActionTask : ActionTreeTask
    {
        //what message we should log
        public string messageToLog = "Log Message";

        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if(hasReadyResult(controller)) return controller.executeStack[TaskIndex];
            Debug.Log(messageToLog);
            //cache result
            controller.executeStack[TaskIndex] = TaskResult.SUCCESS;
            return TaskResult.SUCCESS;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("messageToLog"));
            return list;
        }

#endif
    }

}
