using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    [System.Serializable]
    public abstract class BaseEvent
    {
        public BaseEvent(GameEventID id)
        {
            GetEventID = id;
            DispatchGlobally = id.dispatchedGlobally;
            EventName = id.eventName;
        }
        public string EventName { get; set; } = "BaseEvent";
        public float EventDispatchTime { get; set; } = 0.0f;
        public bool DispatchGlobally { get; set; } = false;
        public object Sender { get; set; } = null;
        public GameEventID GetEventID { get; private set; }
    }

    public class AttackTargetEvent : Base.Events.BaseEvent
    {
        public Transform attackTarget;
        public AttackTargetEvent(GameEventID id_):base(id_)
        {
      
        }
    }
    public class DealDamageEvent: Base.Events.BaseEvent
    {
        public Transform damageTarget;
        public Game.DamageSourceInfo damageValue;
        public DealDamageEvent(GameEventID id_) : base(id_)
        {
           
        }
    }
    public class HitByMissleEvent : Base.Events.BaseEvent
    {
        public Game.DamageSourceInfo damageValue;
        public Vector3 hitPoint;
        public bool dealtDamage;
        public float rawDamageValue;
        public bool getStrongerByThisHit = false;
        public HitByMissleEvent(GameEventID id_) : base(id_)
        {
           
        }
    }
    public class KilledEvent : Base.Events.BaseEvent
    {
        public bool isPlayer = false;
        public KilledEvent(GameEventID id_) : base(id_)
        {
           
        }
    }
    public class GameOverEvent : Base.Events.BaseEvent
    {
        public bool playerFailed = false;
        public GameOverEvent(GameEventID id_) : base(id_)
        {
           
        }
    }
}