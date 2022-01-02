using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Procedural.Terrain
{
    public class TestNoiseCreator : MonoBehaviour
    {
        public enum ShowType
        {
            NOISE,
            COLOR,
            MESH
        }

        #region PUBLIC PARAMS
        public Base.Procedural.MeshHelper.BaseMeshController terrainMeshControl;
        public AnimationCurve heightMultiplierCurve;
        public float heightMultiplier = 1.0f;
        public ColorHeightmapTexture.TerrainType[] terrainTypes;
        public ShowType showType;
        public int terrainChunkSize = 241;
        [Range(0,6)]
        public int LOD = 1;
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
            if (noiseTexture == null)
            {
                //noiseTexture= Base.Procedural.Noise.Noise.CreateNoiseTexture(textureSizeWidth, textureSizeHeight,seed, scaling,octaves,lacunarity,persistance,offset);
                noiseTexture = Base.CommonCode.Common.createTexture(terrainChunkSize, terrainChunkSize);
                noiseTexture.filterMode = FilterMode.Point;
            }
            else
            {
                //Base.Procedural.Noise.Noise.FillTextureWithPerlinNoise(noiseTexture,seed, scaling, octaves, lacunarity, persistance,offset);
                noiseMap = Base.Procedural.NoiseHelpers.Noise.GetPerlinNoiseMap(terrainChunkSize, terrainChunkSize, seed, scaling, octaves, lacunarity, persistance, offset);
                if (showType == ShowType.COLOR)
                {
                    colorMap();
                }
                else if(showType== ShowType.MESH)
                {
                    colorMap();
                    MeshHelper.MeshData data=Base.Procedural.Creator.ProceduralCreator.CreateMeshDataFromHeightMap(noiseMap, LOD,Vector3.zero, heightMultiplier,heightMultiplierCurve,1.0f);
                    terrainMeshControl.getFilter().sharedMesh = data.getMesh(true);
                    terrainMeshControl.getRenderer().sharedMaterial.mainTexture = noiseTexture;
                }
                else
                {
                    Base.Procedural.NoiseHelpers.Noise.fillTextureWithNoiseData(noiseTexture, noiseMap);
                }

            }
            mat.mainTexture = noiseTexture;
        }

        private void colorMap()
        {
            ColorHeightmapTexture.fillTextureWithTerrainColors(terrainTypes, noiseMap, noiseTexture);
        }
    }
}