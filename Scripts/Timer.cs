//K. Homa 27.02.2021
using UnityEngine;

namespace Base.TimeControl
{

    public class Timer : MonoBehaviour
    {
        public static float deltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }

        public static float time
        {
            get
            {
                return Time.time;
            }
        }

        public static float fixedDeltaTime
        {
            get
            {
                return Time.fixedDeltaTime;
            }
        }

        public static float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
        }

        public static float computeFloat(float in_)
        {
            return in_ * deltaTime;
        }


        public static float computeFixedFloat(float in_)
        {
            return in_ * fixedDeltaTime;
        }

    }
}
