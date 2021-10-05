//K. Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.ObjectsControl
{

    public class ObjectsManagerController : MonoBehaviour
    {
        public List<ObjectsManager> managers = new List<ObjectsManager>();
        public bool isPaused = false;
        protected virtual void Start()
        {
            onInit();
        }

        protected virtual void OnDestroy()
        {
            foreach (ObjectsManager mgr in managers)
            {
                mgr.onDestroy();
                mgr.listeners.clear();
            }
        }

        protected virtual void Update()
        {
            if (isPaused) return;
            for (int a = 0; a < managers.Count; ++a)
            {
                managers[a].onUpdate();
            }
        }

        public virtual void onInit()
        {
            foreach (ObjectsManager mgr in managers)
            {
                mgr.onInit();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (isPaused) return;
            for (int a = 0; a < managers.Count; ++a)
            {
                managers[a].onFixedUpdate();
            }
        }

        public virtual void onChangeState(GameSystem.GameState newState)
        {
            if (newState == GameSystem.GameState.PLAY) isPaused = false;
            else
            {
                isPaused = true;
            }

            foreach (ObjectsManager mgr in managers)
            {
                mgr.onChangeState(newState);
            }

        }
    }
}
