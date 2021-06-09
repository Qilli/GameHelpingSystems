using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    public interface IEventListener
    {
        void onEvent(BaseEvent eventObj);
        int getEventCategory();
    }

    public class EventsController
    {
        #region PARAMS
        public List<IEventListener> listeners = new List<IEventListener>();
        public int Category { get; } = 0;
        #endregion

        #region CONTROL
        public EventsController(int category)
        {
            Category = category;
        }

        public void addListener(IEventListener listener) => listeners.Add(listener);
        public bool removeListener(IEventListener listener) => listeners.Remove(listener);
       
        public virtual void executeEvent(BaseEvent event_)
        {
            foreach(IEventListener listener in listeners)
            {
                listener.onEvent(event_);
            }
        }
        #endregion

        #region PRIVATE
        #endregion

    }
}
