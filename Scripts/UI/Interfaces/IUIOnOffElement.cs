using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.UI
{
    public interface IUIOnOffElement
    {
        void turnOnElement();
        void turnOnElementInstantly();
        void turnOffElement();
        void turnOffElementInstantly();
    }
}
