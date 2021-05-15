using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    [System.Serializable]
    public abstract class BaseEvent
    {
        public string EventName { get; set; } = "BaseEvent";
        public float EventDispatchTime { get; set; } = 0.0f;
        public bool DispatchGlobally { get; set; } = false;
        public int Category { get; set; } = EventsManager.GlobalEventsCategory;
        public object Sender { get; set; } = null;
    }

    public class AttackTargetEvent : Base.Events.BaseEvent
    {
        public const int AttackEventCategory = 1;
        public AttackTargetEvent()
        {
            // Base.Events.GameEvent
        }
    }
}