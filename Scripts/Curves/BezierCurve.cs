using UnityEngine;
using UnityEditor;
namespace Base.Curves
{
    [System.Serializable]
    public class BezierCurve: ICurve
    {
        #region PUBLIC PARAMS
        private Vector3[] points = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        #endregion
        #region PRIVATE PARAMS

        #endregion

        #region PUBLIC FUNC
        public Vector3 getPointOnCurve(float t)
        {
            //lerp a-b,b-c,c-d
            Vector3 a = Vector3.Lerp(points[0], points[1], t);
            Vector3 b = Vector3.Lerp(points[1], points[2], t);
            Vector3 c = Vector3.Lerp(points[2], points[3], t);
            //a-b b-c
            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);
            //d-e
            return Vector3.Lerp(d, e, t);
        }
        public float getApproxDistanceTo(float t, int steps)
        {
            Debug.Assert(steps > 1, "Steps need to be > 1");
            Vector3 point0 = getPointOnCurve(0);
            float dist = 0;
            for(int a=0;a<steps;++a)
            {
                float value = a / (steps - 1) * t;
                Vector3 point1 = getPointOnCurve(value);
                dist += Vector3.Distance(point0, point1);
                point0 = point1;
            }
            return dist;
        }

        public float getApproxLength(int steps)
        {
            Debug.Assert(steps > 1,"Steps need to be > 1");
            Vector3 point0 = getPointOnCurve(0);
            float dist = 0;
            for (int a=1;a<steps;++a)
            {
                float value = a / (steps-1);
                Vector3 point1 = getPointOnCurve(value);
                dist += Vector3.Distance(point0, point1);
                point0 = point1;
            }
            return dist;
        }

        public Vector3 getCurveTangentAt(float t)
        {
            //lerp a-b,b-c,c-d
            Vector3 a = Vector3.Lerp(points[0], points[1], t);
            Vector3 b = Vector3.Lerp(points[1], points[2], t);
            Vector3 c = Vector3.Lerp(points[2], points[3], t);
            //a-b b-c
            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);
            return (e - d).normalized;
        }
        public Quaternion getCurveOrientationAt(float t)
        {
            Vector3 forwardDir = getCurveTangentAt(t);
            return Quaternion.LookRotation(forwardDir);
        }
        public Vector3 getPointAt(int index)
        {
            Debug.Assert(index >= 0 && index < 4, "Invalid bezier curve index");
            return points[index];
        }

        public void setPointAt(int index, in Vector3 pos)
        {
            Debug.Assert(index >= 0 && index < 4, "Invalid bezier curve index");
            points[index] = pos;
        }

        public void debugEditorDraw()
        {
#if UNITY_EDITOR
            Handles.DrawBezier(getPointAt(0), getPointAt(3), getPointAt(1), getPointAt(2), Color.white, EditorGUIUtility.whiteTexture, 1f);
            for (int i = 0; i < 4; i++)
            {
                if (i==0||i==3) Gizmos.color = Color.blue;
                else Gizmos.color = Color.red;
                Gizmos.DrawSphere(getPointAt(i), 0.1f);
            }
#endif
        }

        #endregion
        #region PRIVATE FUNC
        #endregion
    }
}
