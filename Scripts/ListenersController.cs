using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{

    [System.Serializable]
    public class ListenersControler<T>
    {
        [SerializeField]
        public List<T> listeners = new List<T>();
        public delegate void onEachListener(T obj);

        public virtual void onEach(onEachListener exec)
        {
            foreach (T listener in listeners)
            {
                exec(listener);
            }
        }

        public virtual void addListener(T obj)
        {
            listeners.Add(obj);
            listeners.Sort();
        }

        public virtual void removeListener(T obj)
        {
            listeners.Remove(obj);
        }

        public virtual void clear()
        {
            listeners.Clear();
        }
    }
}
