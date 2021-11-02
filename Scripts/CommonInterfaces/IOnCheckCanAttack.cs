
using UnityEngine;
using Base.AI.Agents;
namespace Base.CommonInterfaces
{
    public interface IOnCheckCanAttack 
    {
        public bool canAttack(GameplayObjectInfo target, GameplayObjectInfo attacker);
    }
}
