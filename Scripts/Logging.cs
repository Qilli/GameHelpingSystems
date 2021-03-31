//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


namespace Base.Log
{

    public enum BaseLogType
    {
        INFO,
        WARNING,
        ERROR
    }

    public static class Logging
    {
        [Conditional("DEBUG_LOG_ON")]
        public static void Log(string message_, BaseLogType type = BaseLogType.INFO)
        {
            UnityEngine.Debug.Log(message_);
        }

    }
}
