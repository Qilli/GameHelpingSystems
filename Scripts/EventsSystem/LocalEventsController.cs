using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    public class LocalEventsController : MonoBehaviour
    {
        private EventsController localEventsController=new EventsController(0);
        public EventsController GetController {get{return localEventsController;} }
    }
}
