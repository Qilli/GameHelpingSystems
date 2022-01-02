using UnityEngine;

namespace Base.Procedural.Terrain.NoiseData
{
    [System.Serializable]
    public class NoiseData
    {
        public int noiseSize = 241;
        public float scaling = 1;
        public int octaves = 3;
        public int seed = 1345;
        public Vector2 offset;
        public float lacunarity = 2;
        public float persistance = 0.25f;

        private float[,] noiseData = null;

        public void generateNoiseData()
        {
            noiseData = Base.Procedural.NoiseHelpers.Noise.GetPerlinNoiseMap(noiseSize,noiseSize, seed, scaling, octaves, lacunarity, persistance, offset);
        }

        public float[,] getNoiseData() => noiseData;
    }
}
