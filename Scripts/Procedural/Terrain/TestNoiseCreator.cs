using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNoiseCreator : MonoBehaviour
{
    [System.Serializable]
    public struct TerrainType
    {
        public float height;
        public Color color;
    }

    #region PUBLIC PARAMS
    public TerrainType[] terrainsType;
    public int textureSizeWidth = 256;
    public int textureSizeHeight = 256;
    public float scaling = 1;
    public int octaves = 3;
    public int seed = 1345;
    public Vector2 offset;
    public float lacunarity = 2;
    public float persistance = 0.25f;
    #endregion
    #region PRIVATE PARAMS
    private Material mat;
    private Texture2D noiseTexture;
    private float[,] noiseMap;
    #endregion

    #region PUBLIC FUNC
    #endregion
    #region PRIVATE FUNC
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(noiseTexture==null)
        {
            //noiseTexture= Base.Procedural.Noise.Noise.CreateNoiseTexture(textureSizeWidth, textureSizeHeight,seed, scaling,octaves,lacunarity,persistance,offset);
            noiseTexture = Base.CommonCode.Common.createTexture(textureSizeWidth, textureSizeHeight);
        }
        else
        {
            //Base.Procedural.Noise.Noise.FillTextureWithPerlinNoise(noiseTexture,seed, scaling, octaves, lacunarity, persistance,offset);
            noiseMap = Base.Procedural.NoiseHelpers.Noise.GetPerlinNoiseMap(textureSizeWidth, textureSizeHeight, seed, scaling, octaves, lacunarity, persistance, offset);
            colorMap();

        }
        mat.mainTexture = noiseTexture;
    }

    private void colorMap()
    {
        for (int i = 0; i < textureSizeWidth; i++)
        {
            for (int j = 0; j < textureSizeHeight; j++)
            {

            }
        }
    }
}
