using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public class CharacterBaseLifeControl : MonoBehaviour,IIsAlive,Events.IOnEvent,ILifePointsControl
    {
        [System.Serializable]
        public class DamageVunerable
        {
            public DamageSourceInfo.DamageSource forDamageSource;
            [Tooltip("1 -> 100% 0 -> 0% -1->-100%")]
            public float percReaction = 1.0f; // 0.0 -> no result, 1.0 -> 100%, -1.0f -> deal no damage, on the contrary
            public bool vunerableTo(DamageSourceInfo.DamageSource source)
            {
                return forDamageSource == source&&percReaction>0.0f;
            }
            public bool getStrongerBy(DamageSourceInfo.DamageSource source)
            {
                return forDamageSource == source && percReaction < 0.0f;
            }
        }

        [Header("Base Info")]
        public float healthValue = 100;
        public float maxHealthValue = 150;
        public DamageVunerable[] vunerablesList = null;
        [Header("Life effects")]
        public Base.Game.DynamicObjectEffect[] deathEffects = null;
        public Base.Game.DynamicObjectEffect[] bornEffects = null;
        [Header("Death listeners")]
        public Base.Game.IOnLifeChangeActions[] lifeListeners = null;
        protected Base.AI.Agents.GameplayObjectInfo info;
        [Header("On Event response")]
        public Base.Events.GameEventID dealDamageEventID;
        #region PUBLIC
        public bool isAlive()
        {
            return healthValue > 0;
        }
        public virtual void onEventResponse(Base.Events.BaseEvent damageEvent)
        {
            Debug.Log("OnDamageReceived!");
        }
        public virtual bool hasEventID(Base.Events.GameEventID eventID)
        {
            return eventID == dealDamageEventID;
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
        public virtual void removeLifePoints(float lifePoints)
        {
            healthValue -= lifePoints;
            healthValue = Mathf.Clamp(healthValue,0,maxHealthValue);
            checkIfWeNeedToKillCharacter();
        }
        public virtual void addSomeLifePoints(float lifePoints)
        {
            healthValue += lifePoints;
            healthValue = Mathf.Clamp(healthValue, 0, maxHealthValue);
        }
        public virtual void addSomeLifePointsPerc(float lifePointsPerc)//perc in 0-1
        {
            addSomeLifePoints(healthValue * Mathf.Clamp01(lifePointsPerc));
        }

        public void removeLifePointsPerc(float lifePointsPerc)
        {
            throw new System.NotImplementedException();
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
        public virtual bool isMadeStrongerByThisDamage(DamageSourceInfo source)
        {
            foreach (DamageVunerable dv in vunerablesList)
            {
                foreach (DamageSourceInfo.SourceValue sourceDamage in source.damageSources)
                {
                    if (dv.getStrongerBy(sourceDamage.source))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        protected void Awake()
        {
            //find all death listeners
            lifeListeners = this.GetComponentsInChildren<Base.Game.IOnLifeChangeActions>();
            info = GetComponent<Base.AI.Agents.GameplayObjectInfo>();
            showOnBornEffects();
        }
        #region PROTECTED

        protected float dealDamageWithSource(DamageSourceInfo.SourceValue source)
        {
            float damageDealt = 0;
            foreach(DamageVunerable dv in vunerablesList)
            {
                if (dv.vunerableTo(source.source))
                {
                    removeLifePoints(dv.percReaction * source.value);
                    damageDealt += dv.percReaction * source.value;
                }
            }
            return damageDealt;
        }

        protected virtual void checkIfWeNeedToKillCharacter()
        {
            if(!isAlive())
            {
                if(lifeListeners!=null)
                {
                    foreach(Base.Game.IOnLifeChangeActions pre in lifeListeners)
                    {
                        pre.onPreDestroy();
                    }
                }
            }
        }

        protected virtual void showOnBornEffects()
        {
            if (lifeListeners != null)
            {
                foreach (Base.Game.IOnLifeChangeActions pre in lifeListeners)
                {
                    pre.onBorn();
                }
            }
            //show death effects
            if (bornEffects != null)
            {
                foreach (Base.Game.DynamicObjectEffect obj in bornEffects)
                {
                    Base.Game.DynamicObjectEffect.createEffect(obj, info.objectTransformID.position);
                }
            }
        }
        #endregion
    }
}
