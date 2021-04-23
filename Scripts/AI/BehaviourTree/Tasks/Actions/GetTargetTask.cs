using System.Collections;
using System.Collections.Generic;
using Base.AI.Agents;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours
{
    public class GetTargetTask : ActionTreeTask
    {
        //params
        public string targetTag = "Tag";
        public string targetName = "Target Name";
        public bool useTag = false;
        public bool useName = false;
        public EditorSharedVariable target= new EditorSharedVariable() { name = "", type = SharedVariable.SharedType.TRANSFORM};

        public override BehaviourTreeTaskRuntime getRuntimeTask(BehaviourTreeController runtimeController,BehaviourTreeTaskRuntime parent=null)
        {
            GetTargetTaskRuntime rn = new GetTargetTaskRuntime();
            rn.target = runtimeController.Blackboard.getVariableByName(target.name) as SharedTransform;
            rn.type = TaskType.ACTION;
            rn.parent = parent;
            //parameters
            rn.targetTag = targetTag;
            rn.targetName = targetName;
            rn.useTag = useTag;
            rn.useName = useName;
            return rn;
        }

#if UNITY_EDITOR

        public override List<SerializedProperty> getAllProperties(SerializedObject obj)
        {
            List<SerializedProperty> list = new List<SerializedProperty>();
            list.Add(obj.FindProperty("targetTag"));
            list.Add(obj.FindProperty("useTag"));
            list.Add(obj.FindProperty("targetName"));
            list.Add(obj.FindProperty("useName"));
            list.Add(obj.FindProperty("target"));
            return list;
        }

#endif
    }

    public class GetTargetTaskRuntime: ActionTreeTaskRuntime
    {
        public string targetTag = "Tag";
        public string targetName = "Target Name";
        public bool useTag = false;
        public bool useName = false;
        public SharedTransform target;
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            if (hasReadyResult(controller)) return lastResult;

            if (target.value != null)
            {
                return lastResult = TaskResult.SUCCESS;
            }

            GameObject found=null;
            //find target
            if(useTag)
            {
                found = GameObject.FindGameObjectWithTag(targetTag);
                if (found != null)
                {
                    if (useName && !found.name.Equals(targetName))
                    {
                        return lastResult=TaskResult.FAIL;
                    }
                    target.value = found.transform;
                    return lastResult = TaskResult.SUCCESS;
                }
            }

            if(useName)
            {
                found = GameObject.Find(targetName);
                if(found!=null)
                {
                    target.value = found.transform;
                    return lastResult = TaskResult.SUCCESS;
                }
                return lastResult = TaskResult.FAIL;
            }
            
            //cache result
            return lastResult = TaskResult.FAIL;
        }
    }

}
