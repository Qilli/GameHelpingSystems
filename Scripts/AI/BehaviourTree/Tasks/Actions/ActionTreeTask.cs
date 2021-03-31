using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    public class ActionTreeTask : BehaviourTreeTask
    {
        public virtual void init(Agents.AIAgent agent) { }
        public virtual void onTaskStart(Agents.AIAgent agent) { }
        public virtual void onTaskEnd(Agents.AIAgent agent) { }
#if UNITY_EDITOR

        public virtual List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            return new List<SerializedProperty>();
        }

#endif
    }

}
