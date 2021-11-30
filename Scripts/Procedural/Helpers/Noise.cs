using UnityEngine;

namespace Base.Procedural.NoiseHelpers
{
    public class Noise
    {
        public static Texture2D CreateNoiseTexture(int width, int height,int seed, float scaling, int octaves, float lacunarity, float persistance,Vector2 offset)
        {
            Texture2D noiseTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
            FillTextureWithPerlinNoise(noiseTexture, seed,scaling, octaves, lacunarity, persistance,offset);
            return noiseTexture;
        }

        public static void FillTextureWithPerlinNoise(Texture2D tex,int seed, float scaling, int octaves, float lacunarity, float persistance,Vector2 offset)
        {
            int width = tex.width;
            int height = tex.height;
            float[,] map = GetPerlinNoiseMap(width, height,seed, scaling, octaves, lacunarity, persistance,offset);
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Color resColor = Color.Lerp(Color.black, Color.white, map[x, y]);
                    tex.SetPixel(x, y, resColor);
                }
            }
            tex.Apply();
        }

        public static float[,] GetPerlinNoiseMap(int width, int height,int seed, float scaling, int octaves, float lacunarity, float persistance,Vector2 offset)
        {
            float[,] perlinMap = new float[width, height];
            System.Random rng = new System.Random(seed);
            Vector2[] octavesRandom = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                octavesRandom[i].x = rng.Next(-100000,100000)+offset.x;
                octavesRandom[i].x = rng.Next(-100000, 100000)+offset.y;
            }

            float heightOffset = height / 2.0f;
            float widthOffset = width / 2.0f;

            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    float frequency = 1;
                    float amplitude = 1;
                    float heightValue = 0;
                    for (int a = 0; a < octaves; a++)
                    {
                        heightValue += (PerlinNoise(x-widthOffset, y-heightOffset, frequency, scaling,octavesRandom[a]) * 2.0f - 0.5f) * amplitude;
                        frequency *= lacunarity;
                        amplitude *= persistance;
                    }
                    if (heightValue > maxHeight) maxHeight = heightValue;
                    else if (heightValue < minHeight) minHeight = heightValue;
                    perlinMap[x, y] = heightValue;
                }
            }

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    perlinMap[x, y] = Base.CommonCode.MathHelpers.MathHelpers.InverseLerp(minHeight, maxHeight, perlinMap[x, y]);
                }
            }

            return perlinMap;
        }

        public static float PerlinNoise(float x, float y, float frequency, float scaling,Vector2 offset)
        {
            x = (x / scaling) * frequency +offset.x;
            y = (y / scaling) * frequency +offset.y;
            return Mathf.PerlinNoise(x, y);
        }

    }
}
