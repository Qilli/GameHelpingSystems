//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Base.UI
{
    public class onClickElement : MonoBehaviour, IPointerClickHandler
    {
        public EventVector2 onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke(transform.position);

        }
    }
}
