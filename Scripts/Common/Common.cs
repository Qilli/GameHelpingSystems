//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Base.CommonCode
{

    public class Common : MonoBehaviour
    {
        public enum DamageSource
        {
            ALL,
            FIRE,
            WATER,
            EARTH,
            FORCE
        }


        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            //some code here that uses something from the UnityEditor namespace

            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;

#else
        return null;
#endif
        }

        public static bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        public static Vector3 getFireDirection(Vector3 startPos, Vector3 endPos, float speed, Vector3 g)
        {
            Vector3 gravity = g;
            Vector3 direction = Vector3.zero;
            Vector3 delta = endPos - startPos;
            float a = Vector3.Dot(gravity, gravity);
            float b = -4 * (Vector3.Dot(gravity, delta) + speed * speed);
            float c = 4 * Vector3.Dot(delta, delta);
            if (4 * a * c > b * b)
            {
                return direction;
            }
            float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
            float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));

            float time;
            if (time0 < 0.0f)
            {
                if (time1 < 0.0f)
                {
                    return direction;
                }
                time = time1;

            }
            else
            {
                if (time1 < 0)
                {
                    time = time0;
                }
                else
                {

                    time = Mathf.Min(time0, time1);
                    if (time > 2.0f || time < 1.0f)
                    {
                        time = Mathf.Max(time0, time1);
                    }
                }
            }

            if (time > 2.0f || time < 1.0f)
            {
                return direction;
            }

            direction = 2 * delta - gravity * (time * time);
            direction = direction / (2 * speed * time);
            return direction;
        }

        public static bool areVectorsEqual(Vector3 one, Vector3 two)
        {
            if (Mathf.Approximately(one.x, two.x) && Mathf.Approximately(one.y, two.y) && Mathf.Approximately(one.z, two.z))
            {
                return true;
            }
            return false;
        }
        public static class FadeAudioSource
        {
            public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
            {
                float currentTime = 0;
                float start = audioSource.volume;

                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                    yield return null;
                }
                yield break;
            }
        }
    }
    // Helper Rect extension methods
    public static class RectExtensions
    {
        public static Vector2 TopLeft(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }
        public static Rect ScaleSizeBy(this Rect rect, float scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }
        public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }
        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }
        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }
    }

    public class EditorZoomArea
    {
        private const float kEditorWindowTabHeight = 21.0f;
        private static Matrix4x4 _prevGuiMatrix;

        public static Rect Begin(float zoomScale, Rect screenCoordsArea)
        {
            GUI.EndGroup();        // End the group Unity begins automatically for an EditorWindow to clip out the window tab. This allows us to draw outside of the size of the EditorWindow.

            Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.TopLeft());
            clippedArea.y += kEditorWindowTabHeight;
            GUI.BeginGroup(clippedArea);

            _prevGuiMatrix = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
            GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

            return clippedArea;
        }

        public static void End()
        {
            GUI.matrix = _prevGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, kEditorWindowTabHeight, Screen.width, Screen.height));
        }
    }

    //singleton helper
    public class SingletonBase<T> : MonoBehaviour where T:MonoBehaviour
    {
        private static T container=null;

        public static T It
        {
            get
            {
                if (container == null)
                {
                    container = GameObject.FindObjectOfType<T>();
                }

#if UNITY_EDITOR

                if (container == null)
                {
                    Base.Log.Logging.Log("Wrong initialization order!");
                    Debug.DebugBreak();
                }

#endif
                return container;
            }
        }
    }

}
