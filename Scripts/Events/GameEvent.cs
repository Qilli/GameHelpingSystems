//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    [CreateAssetMenu(menuName = "EngineAssets/System/Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        protected List<GameEventListener> listeners = new List<GameEventListener>();
        public EventParams eventParams = new EventParams();

        public virtual void Raise()
        {
            if (listeners.Count == 0) return;
            for (int a = listeners.Count - 1; a >= 0; --a)
            {
                listeners[a].onEventRaised(eventParams);
            }
        }

        public void registerListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void unregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
