using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Events
{
    public interface IOnEvent
    {
        void onEventResponse(BaseEvent event_);
        bool hasEventID(Base.Events.GameEventID eventID);
    }
}
