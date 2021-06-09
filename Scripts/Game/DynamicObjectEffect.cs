using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    [System.Serializable]
    public class DynamicObjectEffect
    {
        public GameObject template;
        public float lifeTime = 0;//0->forever
        public Vector3 initPosOffset = Vector3.zero;
    }
}
