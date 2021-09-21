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
            return default;
        }
        #endregion
        #region PRIVATE FUNC
        #endregion
    }
}

