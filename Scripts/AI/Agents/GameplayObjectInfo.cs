using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Agents
{
    public class GameplayObjectInfo : MonoBehaviour
    {
        public enum GameplayObjectType
        {
            DEFAULT,
            AIAGENT,
            PLAYER
        }

        private string objectName;
        private readonly GameplayObjectType objectType;

        public string Name => objectName;

        public GameplayObjectType Type => objectType;

        private void Awake()
        {
            objectName = this.gameObject.name;
        }

        public GameplayObjectInfo()
        {
            objectType = GameplayObjectType.DEFAULT;
        }

        public GameplayObjectInfo(string name_,GameplayObjectType type_)
        {
            objectName = name_;
            objectType = type_;
        }

        public GameplayObjectInfo(GameplayObjectType type_)
        {
            objectType = type_;
        }

        public virtual Vector3 getTargetPosition()
        {
            return transform.position;
        }
    }

}
