using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.UI
{
    public interface IUIElementLife
    {
        void onBorn();
        void onDeath();
    }
}
