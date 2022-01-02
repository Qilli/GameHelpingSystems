using UnityEngine;

namespace Base.Procedural.Terrain.TerrainData
{
    [System.Serializable]
    public class TerrainData
    {
        [Header("Parameters")]
        public AnimationCurve heightMultiplierCurve;
        public float heightMultiplier = 1.0f;
        public int terrainChunkSize = 241;
        [Range(0, 6)]
        public int LOD = 1;

        private Procedural.MeshHelper.MeshData meshData = null;
        private Terrain.NoiseData.NoiseData noiseData = null;

        public void setUsedNoiseData(Terrain.NoiseData.NoiseData noise) => noiseData = noise;

        public void generateTerrainMeshData()
        {
            if(noiseData!=null)
            {
                
            }
        }
        public Procedural.MeshHelper.MeshData getMeshData() => meshData;
    }
}
