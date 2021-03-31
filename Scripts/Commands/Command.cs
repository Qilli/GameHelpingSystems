//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Commands
{
    public enum CommandResult
    {
        DONE,
        FAILED,
        FAILED_CANCEL,
        WORKING,
        NEW
    }

    [System.Serializable]
    public enum CommandType
    {
        NONE = -1,
        DEFAULT = 0,
    }

    public abstract class Command
    {
        public abstract CommandResult execute();
        public abstract void onCancel();
        public abstract CommandType getCommandType();
    }
}
