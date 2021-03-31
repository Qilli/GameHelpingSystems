//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Base.UI
{

    [System.Serializable]
    public class EventVector2 : UnityEvent<Vector2> { }
    [System.Serializable]
    public class EventVector2Int : UnityEvent<Vector2, int> { }

    public class OnHoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public EventVector2 onEnter;
        public int index = 0;
        public EventVector2Int onEnterIndex;
        public UnityEvent onExit;
        public UnityEvent onUpdate;

        private bool isOver = false;

        public virtual void OnPointerEnter(PointerEventData pointerEventData)
        {
            onEnter.Invoke(transform.position);
            onEnterIndex.Invoke(transform.position, index);
            isOver = true;
        }

        void FixedUpdate()
        {
            if (isOver)
            {
                onUpdate.Invoke();
            }
        }

        public virtual void OnPointerExit(PointerEventData pointerEventData)
        {
            onExit.Invoke();
            isOver = false;
        }
    }

}
