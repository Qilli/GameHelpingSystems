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

        public static GameObject createEffect(DynamicObjectEffect template_,Vector3 pos)
        {
            GameObject effect = GameObject.Instantiate<GameObject>(template_.template,
            pos + template_.initPosOffset, Quaternion.identity, null
            );
            //set lifetime
            if (template_.lifeTime > 0) GameObject.Destroy(effect, template_.lifeTime);
            return effect;
        }
    }
}
