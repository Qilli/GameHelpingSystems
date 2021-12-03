using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace Base.Editor
{
    public class ProceduralHeightmapGeneratorWindow : UnityEditor.EditorWindow
    {
        private Vector2Int texSize;
        private Material usedMaterial;
        private string currentWarning = " ";
        private Color currentWarningColor = Color.red;
        private Texture2D mapTexture = null;
        private RenderTexture renderTexture = null;
        private Texture2D noiseTextureForHeight=null;
        private Mesh fullScreenQuad = null;
        private int layerRender = 0x10000000;
        [MenuItem("Window/Procedural Heightmap Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ProceduralHeightmapGeneratorWindow));
        }

        private void createTexture()
        {
            if (renderTexture == null) createRenderTexture();
            createTextureBase();
            //set mesh
            renderQuadMesh();
            //copy texture
            createTextureFromRender();
        }
        private void createTextureBase()
        {
            mapTexture = new Texture2D(texSize.x, texSize.y);
        }
        private void createTextureFromRender()
        {
            mapTexture = new Texture2D(texSize.x, texSize.y);
            mapTexture.ReadPixels(new Rect(0, 0, texSize.x, texSize.y), 0, 0);
            mapTexture.Apply();
        }

        private void renderQuadMesh()
        {
            //set noise texture
            usedMaterial.SetPass(0);
            usedMaterial.SetTexture("_NoiseTex",noiseTextureForHeight);
            Graphics.Blit(mapTexture, renderTexture, usedMaterial);
        }

        private void createRenderTexture()
        {
            renderTexture = new RenderTexture(texSize.x, texSize.y, 16);
            renderTexture.Create();
        }

        void OnDestroy()
        {
            if(mapTexture) Texture2D.DestroyImmediate(mapTexture, true);
            if(renderTexture)renderTexture.Release();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            texSize = EditorGUILayout.Vector2IntField("Texture Size:", texSize);
            usedMaterial = (Material)EditorGUILayout.ObjectField(usedMaterial,typeof(Material),false);
            noiseTextureForHeight = (Texture2D)EditorGUILayout.ObjectField(noiseTextureForHeight,typeof(Texture2D),false);
            if (GUILayout.Button("Generate"))
            {
                if (texSize.x <= 0 || texSize.y <= 0)
                {
                    currentWarning = "Wrong texture size! Texture width and height has to be >0";
                    currentWarningColor = Color.red;
                }
                else if(noiseTextureForHeight==null || usedMaterial ==null)
                {
                    currentWarning = "Material or noise texture is equal to null";
                    currentWarningColor = Color.red;
                }
                else
                {
                    createTexture();
                }
            }
            GUILayout.EndVertical();
            Rect uiRect = GUILayoutUtility.GetLastRect();

            if (mapTexture == null)
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
                GUILayout.Box(mapTexture, GUILayout.MaxHeight(512), GUILayout.MaxWidth(512), GUILayout.MinHeight(100), GUILayout.MinWidth(100));
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            if (mapTexture == null)
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
                    if (path.Length != 0)
                    {
                        byte[] data = mapTexture.EncodeToPNG();
                        if (data != null)
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
