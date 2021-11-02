using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{
    public class AgentParameters : MonoBehaviour, IAgentParameters
    {
        [System.Serializable]
        public class Parameters
        {
            public List<IAgentParameters.AgentParam<float>> parametersFloat = new List<IAgentParameters.AgentParam<float>>();
            public List<IAgentParameters.AgentParam<int>> parametersInt = new List<IAgentParameters.AgentParam<int>>();
            public List<IAgentParameters.AgentParam<GameObject>> parametersGO = new List<IAgentParameters.AgentParam<GameObject>>();
            public void set(Behaviours.Blackboard board)
            {
                Behaviours.SharedFloat floatValue=null;
                parametersFloat.ForEach((elem) => { floatValue = (Behaviours.SharedFloat)board.getVariableByName(elem.paramName);floatValue.value = elem.param; }) ;
                Behaviours.SharedInt intValue =null;
                parametersInt.ForEach((elem) => { intValue = (Behaviours.SharedInt)board.getVariableByName(elem.paramName); intValue.value = elem.param; });
                Behaviours.SharedGameObject goValue = null;
                parametersGO.ForEach((elem) => { goValue = (Behaviours.SharedGameObject)board.getVariableByName(elem.paramName); goValue.value = elem.param; });
            }
        }

        [Header("Blackboard")]
        public Behaviours.BehaviourTreeController usedTree;
        [Header("NavMesh")]
        public bool moveByNavMesh=true;
        [Header("Parameters")]
        public Parameters parameters = new Parameters();
        [Header("Stop movement on Hit")]
        public bool stopEnemyMovementWhenHit = false;
        
        public virtual void init()
        {
            setParameters();
            Base.AI.Agents.IOnParamsInit[] result = transform.GetComponentsInChildren<Base.AI.Agents.IOnParamsInit>();
            foreach (Base.AI.Agents.IOnParamsInit elem in result)
            {
                elem.onParamsInit(this);
            }
        }
        public virtual void setParameters()
        {
            parameters.set(usedTree.Blackboard);
        }

        public void setParameter<T>(string name,T value)
        {
            Behaviours.SharedVariable<T> variable =(Behaviours.SharedVariable < T >) usedTree.Blackboard.getVariableByName(name);
            variable.value = value;
        }
        public void setParameter<T>(IAgentParameters.AgentParam<T> param)
        {
            setParameter<T>(param.paramName, param.param);
        }
        public Behaviours.SharedVariable<T> getParameter<T>(string name_)
        {
            return (Behaviours.SharedVariable<T>)usedTree.Blackboard.getVariableByName(name_);
        }
    }
}
