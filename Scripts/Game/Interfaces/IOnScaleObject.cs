using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public interface IOnScaleObject 
    {
        void onScaleUp(float amount);
        void onScaleDown(float amount);
    }
}