using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{
    public class PlayerObjectInfo : GameplayObjectInfo
    {
        [Header("Player Height Offset")]
        public Vector3 centerOffset = Vector3.zero;

        public PlayerObjectInfo():base(GameplayObjectType.PLAYER)
        {

        }

        public override Vector3 getTargetPosition()
        {
            return base.getTargetPosition()+centerOffset;
        }
    }
}
