using UnityEngine;
namespace Base.Curves
{
    public interface ICurve
    {
        public Vector3 getPointOnCurve(float t);
        public float getApproxLength(int steps);
        public float getApproxDistanceTo(float t,int steps);
        public Vector3 getCurveTangentAt(float t);
        public Quaternion getCurveOrientationAt(float t);
        public Vector3 getPointAt(int index);
        public void setPointAt(int index, in Vector3 pos);
        public void debugEditorDraw();
    }
}