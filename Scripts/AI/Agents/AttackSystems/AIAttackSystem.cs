using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.AI.Agents
{
    public abstract class AIAttackSystem : MonoBehaviour
    {
        [Space]
        [Header("check if can attack")]
        public List<GameObject> checkIfCanAttackGO = new List<GameObject>();
        protected List<CommonInterfaces.IOnCheckCanAttack> checkIfCanAttack = new List<CommonInterfaces.IOnCheckCanAttack>();
        private void Awake()
        {
            if (checkIfCanAttackGO.Count > 0) checkIfCanAttackGO.ForEach((e) => checkIfCanAttack.Add(e.GetComponent<CommonInterfaces.IOnCheckCanAttack>()));
        }
        public virtual bool canAttackTarget(GameplayObjectInfo target,GameplayObjectInfo source)
        {
            foreach(CommonInterfaces.IOnCheckCanAttack elem in checkIfCanAttack) if (!elem.canAttack(target, source)) return false;
            return true;
        }
        public abstract void attack(Transform target, Vector3? attackStartPosition = null);
    }
}
