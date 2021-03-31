//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Base.ObjectsControl;

namespace Base.PostProcessing
{
    public class PostProcessControl : BaseObject
    {
        private Volume currentVolume;
        private ColorAdjustments colorAdj;

        public override void init()
        {
            base.init();
            currentVolume = GetComponent<Volume>();

            ColorAdjustments tmp;
            if (currentVolume.profile.TryGet<ColorAdjustments>(out tmp))
            {
                colorAdj = tmp;
            }
        }


        //set on / off color adjustment
        public virtual void setColorAdjustment(bool set)
        {
            if (set) colorAdj.active = true;
            else colorAdj.active = false;
        }

    }
}
