using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{
    public abstract class AIAttackSystem : MonoBehaviour
    {
        public abstract void attack(Transform target, Vector3? attackStartPosition = null);
    }
}
