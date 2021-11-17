using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.CommonCode.MathHelpers
{
    public static class MathHelpers
    {
        public static float InverseLerp(float a,float b,float t)
        {
            return (t - a)/(b - a);
        }
        public static float Remap(float a,float b, float targetA,float targetB,float value)
        {
            float invLerp = InverseLerp(a, b, value);
            return Mathf.Lerp(targetA, targetB, value);
        }
    }
}
