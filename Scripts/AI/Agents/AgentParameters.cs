using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{



    public class AgentParameters : MonoBehaviour
    {
        [System.Serializable]
        public class AgentParam<T>
        {
            public T param = default;
            public string paramName;
        }

        [Header("Blackboard")]
        public Behaviours.BehaviourTreeController usedTree;
        [Header("Base Speed")]
        public AgentParam<float> baseMoveSpeed = default;
        public float baseMoveSpeed_Min = 1.0f;
        public float baseMoveSpeed_Max = 3.0f;
        

        public virtual void setParameters()
        {
        //init move speed
           Behaviours.SharedFloat moveSpeed=(Behaviours.SharedFloat) usedTree.Blackboard.getVariableByName(baseMoveSpeed.paramName);
           moveSpeed.value = baseMoveSpeed.param;
        }

        public void setParameter<T>(string name,T value)
        {
            Behaviours.SharedVariable<T> variable =(Behaviours.SharedVariable < T >) usedTree.Blackboard.getVariableByName(name);
            variable.value = value;
        }
        public void setParameter<T>(AgentParam<T> param)
        {
            setParameter<T>(param.paramName, param.param);
        }
    }
}
