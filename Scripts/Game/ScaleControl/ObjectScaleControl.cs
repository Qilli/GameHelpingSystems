using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public class ObjectScaleControl : MonoBehaviour, IOnScaleObject
    {
        [Header("Parameters")]
        public Transform scaleControlTransform;
        [Range(0.0f, 100.0f)]
        public float maxScaleDownValue = 0;
        [Range(0.0f, 100.0f)]
        public float maxScaleUpValue = 0;
        private float currentAmount = 1.0f;

        void Awake()
        {
            currentAmount = 1.0f;
        }
        public virtual void onScaleDown(float amount)
        {
            amount = getScaleBy(amount, true);
            Vector3 localScale= scaleControlTransform.localScale;
            localScale.x-= amount;
            localScale.y -= amount;
            localScale.z -= amount;
            scaleControlTransform.localScale = localScale;
            currentAmount += amount;
        }

        public virtual void onScaleUp(float amount)
        {
            amount = getScaleBy(amount);
            Vector3 localScale = scaleControlTransform.localScale;
            localScale.x += amount;
            localScale.y += amount;
            localScale.z += amount;
            scaleControlTransform.localScale = localScale;
            currentAmount += amount;
        }

        private float getScaleBy(float amount,bool scalingDown=false)
        {
            if(scalingDown)
            {
                //scaling down
                if (currentAmount - amount >= maxScaleDownValue)
                {
                    //can scale down
                    return amount;
                }
                return currentAmount-maxScaleDownValue;
            }
            else
            {
                //scaling up
                if(currentAmount + amount <=maxScaleUpValue)
                {
                    return amount;
                }return maxScaleUpValue- currentAmount;
            }
        }
    }
}
