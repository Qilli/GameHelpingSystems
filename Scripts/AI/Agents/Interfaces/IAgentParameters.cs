using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{
    public interface IAgentParameters 
    {
        [System.Serializable]
        public class AgentParam<T>
        {
            public T param = default;
            public string paramName;
        }
        public void setParameters();
        public void setParameter<T>(string name, T value);
        public void setParameter<T>(AgentParam<T> param);
    }
}
