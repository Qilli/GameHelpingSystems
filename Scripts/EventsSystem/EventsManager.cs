using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    public class EventsManager : Base.ObjectsControl.BaseObject
    {
        private class EventToExecute
        {
            public BaseEvent eventObj;
            public EventsController ctrl;
            public float timer = 0;
            public EventToExecute(BaseEvent event_,EventsController ctrl_)
            {
                timer = 0;
                eventObj = event_;
                ctrl = ctrl_;
            }
            public void dispatch()
            {
                ctrl.executeEvent(eventObj);
            }
        }

        #region PARAMS
        public const int GlobalEventsCategory = 0;
        //event controllers by events category
        private Dictionary<int, EventsController> eventsControllers = new Dictionary<int, EventsController>();

        private EventsController globalControler = new EventsController(GlobalEventsCategory);
        private List<EventToExecute> executeList = new List<EventToExecute>();
        private List<EventToExecute> toErase = new List<EventToExecute>();
        #endregion

        #region CONTROL
        public EventsManager()
        {
            eventsControllers.Add(GlobalEventsCategory, globalControler);
        }

        public override void onUpdate(float delta)
        {
            base.onUpdate(delta);
            if(executeList.Count>0)
            {
                for(int a=0;a<executeList.Count;++a)
                {
                    executeList[a].timer += delta;
                    if (executeList[a].eventObj.EventDispatchTime<executeList[a].timer )
                    {
                        //instant execute
                        executeList[a].dispatch();
                        toErase.Add(executeList[a]);
                    }
                }

                if(toErase.Count>0)
                {
                    for(int a=0;a<toErase.Count;++a)
                    {
                        executeList.Remove(toErase[a]);
                    }
                    toErase.Clear();
                }
            }
        }

        public virtual bool dispatchEvent(BaseEvent eventObject, bool addCategoryIfNotFound = true)
        {
            if (eventObject.DispatchGlobally)
            {
                executeList.Add(new EventToExecute(eventObject,globalControler));
                return true;
            }
            else
            {
                //find category
                EventsController controller;
                getController(out controller,eventObject.Category, addCategoryIfNotFound);
                if (controller != null)
                {   
                    executeList.Add(new EventToExecute(eventObject,controller));
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
            bool getControllerResult = eventsControllers.TryGetValue(type, out ctrl);
            if (!getControllerResult && addCategoryIfNotFound)
            {
                ctrl = new EventsController(type);
                eventsControllers.Add(type, ctrl);
            }
            else if (!getControllerResult) ctrl = null;
        }
        #endregion
    }

}
