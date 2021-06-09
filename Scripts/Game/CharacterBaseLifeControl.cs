using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public class CharacterBaseLifeControl : MonoBehaviour,IIsAlive
    {
        [System.Serializable]
        public class DamageVunerable
        {
            public DamageSourceInfo.DamageSource forDamageSource;
            [Tooltip("1 -> 100% 0 -> 0%")]
            public float percReaction = 1.0f; // 0.0 -> no result, 1.0 -> 100%, -1.0f -> deal no damage, on the contrary
            public bool vunerableTo(DamageSourceInfo.DamageSource source)
            {
                return forDamageSource == source&&percReaction>0.0f;
            }
        }

        [Header("Base Info")]
        public float healthValue = 100;
        public DamageVunerable[] vunerablesList = null;
        [Header("Life effects")]
        public Base.Game.DynamicObjectEffect[] deathEffects = null;
        [Header("Death listeners")]
        public Base.Game.IOnPreDestroy[] deathListeners = null;
        protected Base.AI.Agents.GameplayObjectInfo info;
        [Header("Deal damage event")]
        public Base.Events.GameEventID damageEventID;
        protected void Awake()
        {
            //find all death listeners
            deathListeners = this.GetComponentsInChildren<Base.Game.IOnPreDestroy>();
            info = GetComponent<Base.AI.Agents.GameplayObjectInfo>();
        }
        public bool isAlive()
        {
            return healthValue > 0;
        }
        public virtual void onDamageReceivedEvent(Base.Events.BaseEvent damageEvent)
        {
            Debug.Log("OnDamageReceived!");
        }
        public virtual bool isHurtByThisDamage(DamageSourceInfo source)
        {
            if(vunerablesList!=null)
            {
                foreach(DamageVunerable dv in vunerablesList)
                {
                    foreach(DamageSourceInfo.SourceValue sourceDamage in source.damageSources)
                    {
                        if(dv.vunerableTo(sourceDamage.source))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }
        public virtual void dealRawDamage(float damage)
        {
            healthValue -= damage;
            healthValue = Mathf.Clamp(healthValue,0,10000);
        }
        public virtual bool tryToDealDamage(DamageSourceInfo source,out float damageDealt)
        {
            damageDealt = 0;
            if(isHurtByThisDamage(source))
            {
                    foreach (DamageSourceInfo.SourceValue sourceDamage in source.damageSources)
                    {
                     damageDealt+=dealDamageWithSource(sourceDamage);
                    }
                checkIfWeNeedToKillCharacter();
                return true;
            }return false;
            
        }

        protected float dealDamageWithSource(DamageSourceInfo.SourceValue source)
        {
            float damageDealt = 0;
            foreach(DamageVunerable dv in vunerablesList)
            {
                if (dv.vunerableTo(source.source))
                {
                    dealRawDamage(dv.percReaction * source.value);
                    damageDealt += dv.percReaction * source.value;
                }
            }
            return damageDealt;
        }

        protected virtual void checkIfWeNeedToKillCharacter()
        {
            if(!isAlive())
            {
                if(deathListeners!=null)
                {
                    foreach(Base.Game.IOnPreDestroy pre in deathListeners)
                    {
                        pre.onPreDestroy();
                    }
                }
            }
        }

        protected void createDeathEffect(Base.Game.DynamicObjectEffect template)
        {
            GameObject effect = GameObject.Instantiate<GameObject>(template.template,
                info.objectTransformID.position + template.initPosOffset, Quaternion.identity, null
                );
            //set lifetime
            if (template.lifeTime > 0) GameObject.Destroy(effect, template.lifeTime);
        }
    }
}
