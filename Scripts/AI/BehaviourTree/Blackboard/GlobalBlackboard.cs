using UnityEngine;
using System.Collections.Generic;
namespace Base.AI.Behaviours
{
    public class GlobalBlackboard : CommonCode.SingletonBase<GlobalBlackboard>
    {
        [System.Serializable]
        protected class GlobalBlackboardData
        {
            public List<SharedGameObject> gameObjectsData;
            public List<SharedTransform> transformObjectsData;
            public List<SharedFloat> floatObjectsData;
            public List<SharedInt> integerObjectsData;
            public List<SharedString> stringObjectsData;
            public List<SharedBool> booleanObjectsData;
            public List<SharedVector> vectorObjectsData;
            
        }
        #region PARAMS
        [SerializeField]
        private GlobalBlackboardData data= new GlobalBlackboardData();
        #endregion
        #region PUBLIC FUNC
        public GameObject getGOParamWithName(string name)
        {
            return data.gameObjectsData.Find((elem) => elem.name == name).value;
        }
        public Transform getTransformParamWithName(string name)
        {
            return data.transformObjectsData.Find((elem) => elem.name == name).value;
        }
        public float getFloatParamWithName(string name)
        {
            return data.floatObjectsData.Find((elem) => elem.name == name).value;
        }
        public int getIntegerParamWithName(string name)
        {
            return data.integerObjectsData.Find((elem) => elem.name == name).value;
        }
        public string getStringParamWithName(string name)
        {
            return data.stringObjectsData.Find((elem) => elem.name == name).value;
        }
        public bool getBooleanParamWithName(string name)
        {
            return data.booleanObjectsData.Find((elem) => elem.name == name).value;
        }
        public Vector3 getVectorParamWithName(string name)
        {
            return data.vectorObjectsData.Find((elem) => elem.name == name).value;
        }
        public bool setGameObjectParam(string name,GameObject go)
        {
            SharedGameObject obj = data.gameObjectsData.Find(elem => elem.name == name);
            if (obj != null)
            {
                obj.value = go;
                return true;
            }
            return false;
        }
        public bool setTransformParam(string name,Transform t)
        {
            SharedTransform obj = data.transformObjectsData.Find(elem => elem.name == name);
            if (obj != null)
            {
                obj.value = t;
                return true;
            }
            return false;
        }
        public bool setFloatParam(string name, float value)
        {
            SharedFloat obj = data.floatObjectsData.Find(elem => elem.name == name);
            if (obj != null)
            {
                obj.value = value;
                return true;
            }
            return false;
        }
        public bool setBooleanParam(string name, bool value)
        {
            SharedBool obj = data.booleanObjectsData.Find(elem => elem.name == name);
            if (obj != null)
            {
                obj.value = value;
                return true;
            }
            return false;
        }
        public bool setIntegerParam(string name, int value)
        {
            SharedInt obj = data.integerObjectsData.Find(elem => elem.name == name);
            if (obj != null)
            {
                obj.value = value;
                return true;
            }
            return false;
        }
        public bool setVectorParam(string name, Vector3 value)
        {
            SharedVector obj = data.vectorObjectsData.Find(elem => elem.name == name);
            if (obj != null)
            {
                obj.value = value;
                return true;
            }
            return false;
        }
        #endregion
        #region PRIVATE FUNC
        #endregion
    }
}

