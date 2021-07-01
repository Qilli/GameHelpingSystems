//K. Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.TimeControl;

namespace Base.ObjectsControl
{
    [CreateAssetMenu(menuName = "EngineAssets/System/ObjectsManager")]
    public class ObjectsManager : ScriptableObject
    {
        public ListenersControler<BaseObject> listeners = new ListenersControler<BaseObject>();
        public bool updateEnabled = true;
        public virtual void onUpdate()
        {
            float currentDelta = Timer.deltaTime;
            if (updateEnabled && Timer.TimeScale != 0)
            {

                for (int a = 0; a < listeners.listeners.Count; ++a)
                {
                    listeners.listeners[a].onUpdate(currentDelta);
                }
            }
        }

        public virtual void onChangeState(GameSystem.GameState newState)
        {
            foreach (BaseObject o in listeners.listeners)
            {
                o.onChangeState(newState);
            }

            if (newState == GameSystem.GameState.END_GAME || newState == GameSystem.GameState.GAME_OVER ||
                newState == GameSystem.GameState.PAUSE) updateEnabled = false;
            else updateEnabled = true;
        }

        public virtual void onDestroy()
        {

        }

        public virtual void onInit()
        {
            for (int a = 0; a < listeners.listeners.Count; ++a)
            {
                listeners.listeners[a].init();
            }
        }

        public virtual void onFixedUpdate()
        {
            float currentDelta = Timer.fixedDeltaTime;
            if (updateEnabled && Timer.TimeScale != 0)
            {

                for (int a = 0; a < listeners.listeners.Count; ++a)
                {
                    listeners.listeners[a].onFixedUpdate(currentDelta);
                }
            }
        }
    }
}