//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Events
{
    [CreateAssetMenu(menuName = "EngineAssets/System/Events/GameEvent")]
    public class GameEventID : ScriptableObject
    {
        public string eventName;
        public bool dispatchedGlobally;
        public int eventCategory;
        public int eventID = 0;
    }
}
