using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    [System.Serializable]
    public class DamageSourceInfo 
    {
        [System.Serializable]
        public enum DamageSource
        {
            DEFAULT,
            FIRE,
            EARTH,
            WATER,
            METAL,
            WOOD
        }
        [System.Serializable]
        public struct SourceValue
        {
            public DamageSource source;
            public float value;
            public SourceValue(DamageSource source_,float value_)
            {
                source = source_;
                value = value_;
            }
        }

        public List<SourceValue> damageSources = new List<SourceValue>();

    }
}

