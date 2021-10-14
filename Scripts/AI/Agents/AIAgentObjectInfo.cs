using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{
    public class AIAgentObjectInfo : GameplayObjectInfo
    {
        [Header("Parameters")]
        public Transform agentGlobalPosition;
        public Vector3 toCenterOffset = Vector3.zero;
        public AIAgentObjectInfo():base(GameplayObjectType.AIAGENT)
        {

        }

        public override Vector3 getTargetPosition()
        {
            return agentGlobalPosition.position+toCenterOffset;
        }
    }
}
