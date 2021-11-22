using System;
using System.Collections;
using UnityEngine;
namespace Base.Curves
{
    [ExecuteInEditMode]
    public class CurvedPathCreator : MonoBehaviour
    {
        #region PUBLIC PARAMS

        #endregion
        #region PRIVATE PARAMS
        [SerializeField]
        private CurvedPath curvedPath= new CurvedPath();
        #endregion

        #region PUBLIC FUNC
        public CurvedPath getPath() => curvedPath;
        public void addNewPoint()
        {
            curvedPath.addSegment(curvedPath.getLastPoint()+Vector3.forward*5.0f);
        }
        public void matchTangents()
        {
            curvedPath.matchTangents();
        }
        public void closePath()
        {
            curvedPath.setClosePath(!curvedPath.ClosedPath);
        }
        #endregion
        #region PRIVATE FUNC
        private void Update()
        {
            if (curvedPath == null) curvedPath = new CurvedPath();
        }
        #endregion
    }
}
