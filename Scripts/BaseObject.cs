//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//test comment to check if i can merge another branch into main
namespace Base.ObjectsControl
{

    public enum ObjectState
    {
        PAUSED,
        WORKING
    }
    //testowy komentarz na main
    //kolejny testowy komentarz
    public class BaseObject : MonoBehaviour, IComparable
    {
        [SerializeField]
        protected ObjectState state;
        public ObjectsManager mgr;
        public int priority = 10;
        public bool inited = false;


        public int CompareTo(object obj)
        {
            BaseObject baseObj = (BaseObject)obj;
            //Negative value means object comes before this in the sort
            //order.
            if (this.priority < baseObj.priority)
                return -1;
            //Positive value means object comes after this in the sort
            //order.
            if (this.priority > baseObj.priority) return 1;
            return 0;
        }

        protected virtual void Awake()
        {
            if (mgr != null)
            {
                mgr.listeners.addListener(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (mgr != null)
            {
                mgr.listeners.removeListener(this);
            }
        }
//KOMENTARZ Z CAPSLOCKIEM
//ponowy komentarz testowy
        public virtual void init()
        {

        }

        public virtual void onUpdate(float delta)
        {

        }

        public virtual void onFixedUpdate(float fixedDelta)
        {

        }

        public virtual void onChangeState(GameSystem.GameState newState)
        {
            switch (newState)
            {
                case GameSystem.GameState.PAUSE:
                case GameSystem.GameState.GAME_OVER:
                case GameSystem.GameState.END_GAME:
                    {
                        state = ObjectState.PAUSED;
                    }
                    break;
                default:
                    {
                        state = ObjectState.WORKING;
                    }
                    break;
            }
        }
    }
}
