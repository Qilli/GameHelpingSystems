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
        }
        public string EventName { get; set; } = "BaseEvent";
        public float EventDispatchTime { get; set; } = 0.0f;
        public bool DispatchGlobally { get; set; } = false;
        public int Category { get; set; } = EventsManager.GlobalEventsCategory;
        public object Sender { get; set; } = null;
        public GameEventID GetEventID { get; private set; }
    }

    public class AttackTargetEvent : Base.Events.BaseEvent
    {
        public Transform attackTarget;
        public AttackTargetEvent(GameEventID id_):base(id_)
        {
            Category = 1;
        }
    }
    public class DealDamageEvent: Base.Events.BaseEvent
    {
        public Transform damageTarget;
        public Game.DamageSourceInfo damageValue;
        public DealDamageEvent(GameEventID id_) : base(id_)
        {
            Category = 1;
        }
    }
    public class HitByMissleEvent : Base.Events.BaseEvent
    {
        public Game.DamageSourceInfo damageValue;
        public Vector3 hitPoint;
        public bool dealtDamage;
        public float rawDamageValue;
        public HitByMissleEvent(GameEventID id_) : base(id_)
        {
            Category = 1;
        }
    }
}