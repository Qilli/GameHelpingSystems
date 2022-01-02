using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Procedural.Terrain
{
    public class ColorHeightmapTexture
    {
        [System.Serializable]
        public struct TerrainType
        {
            public string terrainTypeName;
            public float heightStart;
            public Color terrainColor;
        }

        public static Texture2D fillTextureWithTerrainColors(TerrainType[] terrainTypes, float[,] heightmap, Texture2D usedTexture = null)
        {
            int texWidth = heightmap.GetLength(0);
            int texHeight = heightmap.GetLength(1);
            Texture2D targetTexture = usedTexture ?? Base.CommonCode.Common.createTexture(texWidth, texHeight);
            for (int widthIndex = 0; widthIndex < texWidth; widthIndex++)
            {
                for (int heightIndex = 0; heightIndex < texHeight; heightIndex++)
                {
                    for (int i = 0; i < terrainTypes.Length; i++)
                    {
                        if (terrainTypes[i].heightStart > heightmap[widthIndex, heightIndex])
                        {
                            targetTexture.SetPixel(widthIndex, heightIndex, terrainTypes[i].terrainColor);
                            break;
                        }
                    }
                }
            }
            targetTexture.Apply();
            return targetTexture;
        }
    }
}
