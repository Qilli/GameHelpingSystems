using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    [System.Serializable]
    public class UnityGameBaseEvent: UnityEngine.Events.UnityEvent<BaseEvent>
    {

    }

    public class EventListener : MonoBehaviour, IEventListener
    {
        public GameEventID eventID;
        public UnityGameBaseEvent listeners;

        void OnEnable()
        {
            //register in system
            GlobalDataContainer.It.eventsManager.addListener(this);
        }

        void OnDisable()
        {
            GlobalDataContainer.It.eventsManager.removeListener(this);
        }

        public void onEvent(BaseEvent event_)
        {
         //   listeners?.Invoke(event_);
        }

        public int getEventCategory()
        {
            return (int)eventID.eventCategory;
        }

    }
}
