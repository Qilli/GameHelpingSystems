using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    [System.Serializable]
    public class UnityGameBaseEvent : UnityEngine.Events.UnityEvent<BaseEvent>
    {

    }

    public class EventListener : MonoBehaviour, IEventListener
    {
        [System.Serializable]
        public class EventListenerElement
        {
            public GameEventID eventID;
            public UnityGameBaseEvent listeners;
            public void onInit(Transform t, bool lookInChildren)
            {
                IOnEvent[] onEvents = null;
                if (lookInChildren) onEvents = t.GetComponentsInChildren<IOnEvent>();
                else onEvents = t.GetComponents<IOnEvent>();
                foreach (IOnEvent e in onEvents)
                {
                    if (e.hasEventID(eventID))
                    {
                        listeners.AddListener(e.onEventResponse);
                    }
                }
                onEvents = null;
            }
            public void checkEvent(BaseEvent event_)
            {
                if (eventID.eventID == event_.GetEventID.eventID)
                {
                    listeners?.Invoke(event_);
                }
            }
        }
        public EventListenerElement[] elements;
        public bool lookForListenersInChildren;
        public bool isLocal = false;

        void OnEnable()
        {
            if (isLocal)
            {
                LocalEventsController leventsCtrl = GetComponent<LocalEventsController>();
                if (leventsCtrl == null)
                {
                    leventsCtrl = transform.parent.GetComponent<LocalEventsController>();
                    if (leventsCtrl == null) Debug.LogError("Cannot find local events controller for local listener");
                    else leventsCtrl.GetController.addListener(this);
                }
            }
            else
            {
                //register in system
                GlobalDataContainer.It.eventsManager.addListener(this);
            }

            foreach (EventListenerElement e in elements) e.onInit(transform, lookForListenersInChildren);

        }
        void OnDisable()
        {
            if (isLocal == false && GlobalDataContainer.It != null) GlobalDataContainer.It.eventsManager.removeListener(this);
        }
        public void onEvent(BaseEvent event_)
        {
            foreach (EventListenerElement e in elements) e.checkEvent(event_);
        }
        public int getEventCategory()
        {
            return (elements != null && elements.Length > 0) ? elements[0].eventID.eventID : -1;
        }
    }
}
