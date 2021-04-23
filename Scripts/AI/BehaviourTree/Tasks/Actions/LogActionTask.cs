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
        public EditorSharedVariable logText= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.STRING };

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            LogActionTaskRuntime rn = new LogActionTaskRuntime();
            rn.logText = runtimeController.Blackboard.getVariableByName(logText.name) as SharedString;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.messageToLog = messageToLog;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("messageToLog"));
            list.Add(obj.FindProperty("logText"));
            return list;
        }

#endif
    }

    public class LogActionTaskRuntime: ActionTreeTaskRuntime
    {
        public string messageToLog;
        public SharedString logText;
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;
            Debug.Log(messageToLog+" and from log text: "+logText.value);
            //cache result
            lastResult = TaskResult.SUCCESS;
            return TaskResult.SUCCESS;
        }
    }

}
