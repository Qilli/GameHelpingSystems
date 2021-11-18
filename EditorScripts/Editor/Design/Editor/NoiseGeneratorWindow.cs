using UnityEngine;
using UnityEditor;
using System.IO;
namespace Base.Editor
{
    public class NoiseGeneratorWindow : EditorWindow
    {
        private class NoiseHelper
        {
            private static Vector2 right = new Vector2(1.0f, 0);
            private static Vector2 up = new Vector2(0.0f, 1.0f);
            private static Vector2 diagonalUp = new Vector2(1.0f, 1.0f);
            public static float positionMultiplier = 5.0f;
            public static Vector2 shuffleParameter = new Vector2(12.9898f, 78.233f);
            public static float randomMultiplier = 43758.5453123f;
            static public float simpleNoise(Texture2D tex, int x,int y)
            {
                float xF = widthTo01(tex,x)*positionMultiplier;
                float yF = heightTo01(tex,y)*positionMultiplier;
                Vector2 i = new Vector2(Mathf.Floor(xF), Mathf.Floor(yF));
                Vector2 f = new Vector2(fract(xF), fract(yF));
                float a = random(i);
                float b = random(i+right);
                float c = random(i+up);
                float d = random(i+ diagonalUp);

                //cubioc hermine curve
                Vector2 u = (f * f)*new Vector2(3.0f - 2.0f* f.x,3.0f - 2.0f * f.y);
                return Mathf.Lerp(a, b, u.x) + (c - a) * u.y * (1.0f - u.x) + (d - b) * u.x * u.y;
            }

            static public float widthTo01(Texture2D tex, int x)
            {
                float temp = x;
                return temp / (float)tex.width;
            }
            static public float heightTo01(Texture2D tex, int y)
            {
                float temp = y;
                return temp / (float)tex.height;
            }
            private static float fract(float value)
            {
                value = Mathf.Abs(value);
                return value - Mathf.Floor(value);
            }

            private static float random(Vector2 inData)
            {
                float temp = fract(Mathf.Sin(Vector2.Dot(inData,shuffleParameter)) * randomMultiplier);
                return temp;
            }
            #region Validate
            private static Vector2 getNeighbour(Texture2D tex,int x,int y,int offsetX,int offsetY)
            {
                int finalX = x + offsetX;
                int finalY = y + offsetY;
                if(!isValid(tex,finalX,finalY))
                {
                    if(isValidX(tex,finalX)==false)
                    {
                        wrapX(tex, ref finalX);
                    }
                    if(isValidY(tex,finalY)==false)
                    {
                        wrapY(tex, ref finalY);
                    }

                }
                return new Vector2(widthTo01(tex, finalX), heightTo01(tex, finalY));
            }
            private static bool isValid(Texture2D tex,int x,int y)
            {
                if (x >= 0 && x < tex.width && y >= 0 && y < tex.height) return true;
                return false;
            }

            private static bool isValidX(Texture2D tex,int x)
            {
                return x >= 0 && x < tex.width;
            }
            private static bool isValidY(Texture2D tex, int y)
            {
                return y >= 0 && y < tex.height;
            }
            private static void wrapX(Texture2D tex,ref int x)
            {
                if (x > tex.width) x = 0;
                else x = tex.width - 1;
            }
            private static void wrapY(Texture2D tex, ref int y)
            {
                if (y < 0) y = tex.height - 1;
                else y = 0;
            }
            #endregion
        }


        [MenuItem("Window/Noise Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(NoiseGeneratorWindow));
        }
        private Vector2 texSize;
        private Texture2D noiseTexture = null;
        private string currentWarning = " ";
        private Color currentWarningColor = Color.red;
        private float positionMultiplier = 5.0f;
        private  Vector2 shuffleParameter = new Vector2(12.9898f, 78.233f);
        private  float randomMultiplier = 43758.5453123f;

        private void createTexture()
        {
            noiseTexture = new Texture2D((int)texSize.x, (int)texSize.y, TextureFormat.RGB24, false);
            NoiseHelper.positionMultiplier = positionMultiplier;
            NoiseHelper.shuffleParameter = shuffleParameter;
            NoiseHelper.randomMultiplier = randomMultiplier;
            for (int x = 0; x < (int)texSize.x; ++x)
            {
                for (int y = 0; y < (int)texSize.y; ++y)
                {
                    float resColor = NoiseHelper.simpleNoise(noiseTexture,x,y);
                    noiseTexture.SetPixel(x, y
                        , new Color(resColor,resColor,resColor));
                }
            }
            noiseTexture.Apply();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            texSize = EditorGUILayout.Vector2Field("Texture Size:", texSize);
            positionMultiplier = EditorGUILayout.Slider(positionMultiplier,0.0f,100.0f);
            shuffleParameter = EditorGUILayout.Vector2Field("Shuffle Multiplier: ", shuffleParameter);
            randomMultiplier = EditorGUILayout.FloatField("Random Multiplier: ", randomMultiplier);

            if (GUILayout.Button("Generate"))
            {
                if (texSize.x <= 0 || texSize.y <= 0)
                {
                    currentWarning = "Wrong texture size! Texture width and height has to be >0";
                    currentWarningColor = Color.red;
                }
                else
                {
                    createTexture();
                }
            }
            GUILayout.EndVertical();
            Rect uiRect = GUILayoutUtility.GetLastRect();

            if (noiseTexture == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Box("No Preview");
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Box(noiseTexture, GUILayout.MaxHeight(512), GUILayout.MaxWidth(512), GUILayout.MinHeight(100), GUILayout.MinWidth(100));
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            if (noiseTexture == null)
            {
                GUILayout.BeginVertical();
                GUI.enabled = false;
                GUILayout.Button("Save to File");
                GUI.enabled = true;
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical();
                if (GUILayout.Button("Save to File"))
                {
                    string path = EditorUtility.SaveFilePanel(
                       "Save texture as PNG",
                         "", "NoiseTexture.png", "png");
                    if(path.Length!=0)
                    {
                       byte[] data= noiseTexture.EncodeToPNG();
                        if(data!=null)
                        {
                            File.WriteAllBytes(path, data);
                            currentWarning = "File saved successfuly.";
                            currentWarningColor = Color.white;
                        }
                    }
                    else
                    {
                        currentWarning = "Wrong path!";
                        currentWarningColor = Color.red;
                    }
                }
                GUILayout.EndVertical();
            }

            Color current = GUI.contentColor;
            GUI.contentColor = Color.red;
            GUILayout.Label(currentWarning);
            GUI.contentColor = current;
            GUILayout.EndVertical();
        }
    }
}
