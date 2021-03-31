using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.QTE
{
    public class QTEController : ObjectsControl.BaseObject
    {
        protected List<QuickTimeEvent> events = new List<QuickTimeEvent>();

        public void addAndStartQTEvent(QuickTimeEvent event_)
        {
            event_.start();
            events.Add(event_);
        }

        public void addQTEvent(QuickTimeEvent event_)
        {
            events.Add(event_);
        }


        public override void onUpdate(float delta)
        {
            base.onUpdate(delta);
            if (events.Count > 0)
            {
                for (int a = events.Count - 1; a >= 0; --a)
                {
                    events[a].updateEvent(delta);
                    if (events[a].isOver())
                    {
                        QuickTimeEvent e = events[a];
                        events.RemoveAt(a);
                        e = null;
                    }
                }
            }
        }

    }
}
