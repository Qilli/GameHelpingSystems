using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Animation
{
    public interface IOnAnimationEvent
    {
        public void onAnimationEvent(string eventAnimName);
    }
}
