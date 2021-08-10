using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.UI
{
    public interface IUIElementActivation
    {
        void onActivate();
        void onDeactive();
    }
}
