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
        public bool isLocal = false;

        void OnEnable()
        {
            if (isLocal)
            {
                LocalEventsController leventsCtrl = GetComponent<LocalEventsController>();
                if(leventsCtrl==null)
                {
                    leventsCtrl=transform.parent.GetComponent<LocalEventsController>();
                    if (leventsCtrl == null) Debug.LogError("Cannot find local events controller for local listener");
                    else leventsCtrl.GetController.addListener(this);
                }
            }
            else
            {
                //register in system
                GlobalDataContainer.It.eventsManager.addListener(this);
            }
        }
        void OnDisable()
        {
           if(isLocal==false&&GlobalDataContainer.It!=null) GlobalDataContainer.It.eventsManager.removeListener(this);
        }
        public void onEvent(BaseEvent event_)
        {
            if (eventID.eventID == event_.GetEventID.eventID)
            {
                listeners?.Invoke(event_);
            }
        }
        public int getEventCategory()
        {
            return (int)eventID.eventCategory;
        }
    }
}
