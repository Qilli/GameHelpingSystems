using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public class ObjectScaleControl : MonoBehaviour, IOnScaleObject
    {
        public Transform scaleControlTransform;
        public virtual void onScaleDown(float amount)
        {
            Vector3 localScale= scaleControlTransform.localScale;
            localScale.x-= amount;
            localScale.y -= amount;
            localScale.z -= amount;
            scaleControlTransform.localScale = localScale;
        }

        public virtual void onScaleUp(float amount)
        {
            Vector3 localScale = scaleControlTransform.localScale;
            localScale.x += amount;
            localScale.y += amount;
            localScale.z += amount;
            scaleControlTransform.localScale = localScale;
        }
    }
}
