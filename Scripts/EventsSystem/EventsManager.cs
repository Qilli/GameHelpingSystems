using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    public class EventsManager : Base.ObjectsControl.BaseObject
    {
        #region PARAMS
        public const int GlobalEventsCategory = 0;
        //event controllers by events category
        private Dictionary<int, EventsController> eventsControllers = new Dictionary<int, EventsController>();

        private EventsController globalControler = new EventsController(GlobalEventsCategory);
        #endregion

        #region CONTROL
        public EventsManager()
        {
            eventsControllers.Add(GlobalEventsCategory, globalControler);
        }

        public virtual bool dispatchEvent(BaseEvent eventObject, bool addCategoryIfNotFound = true)
        {
            if (eventObject.DispatchGlobally)
            {
                globalControler.executeEvent(eventObject);
                return true;
            }
            else
            {
                //find category
                EventsController controller;
                getController(out controller,eventObject.Category, addCategoryIfNotFound);
                if (controller != null)
                {
                    controller.executeEvent(eventObject);
                    return true;
                }
                return false;
            }
        }
        public virtual bool addListener(IEventListener listener, bool addCategoryIfNotFound = true)
        {
            return addListenerForCategory(listener, addCategoryIfNotFound);
        }
        public virtual bool removeListener(IEventListener listener)
        {
            //remove from global
            globalControler.removeListener(listener);

            EventsController ctrl;
            getController(out ctrl, listener.getEventCategory(), false);
            if(ctrl!=null)
            {
                return ctrl.removeListener(listener);
            }return false;
        }
        #endregion

        #region PRIVATE
        private bool addListenerForCategory(IEventListener listener, bool addCategoryIfNotFound)
        {
            //always add to global category
            globalControler.addListener(listener);

            EventsController ctrl;
            getController(out ctrl, listener.getEventCategory(), addCategoryIfNotFound);
            if(ctrl!=null)
            {
                ctrl.addListener(listener);
                return true;
            }return false;
        }
        private void getController(out EventsController ctrl,int type, bool addCategoryIfNotFound = true)
        {
            if(!eventsControllers.TryGetValue(type,out ctrl)&&addCategoryIfNotFound)
            {
                ctrl = new EventsController(type);
                eventsControllers.Add(type, ctrl);
            }
            else ctrl = null;
        }
        #endregion
    }

}
