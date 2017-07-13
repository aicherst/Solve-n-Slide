using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EditorExtension {

    public static class TerrainFunctions {
        public static void SwapSplats(Terrain terrain, int splatIndex1, int splatIndex2) {
            TerrainData terrainData = terrain.terrainData;

            float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

            if (splatIndex1 >= alphamaps.GetLength(2) && splatIndex2 >= alphamaps.GetLength(2)) {
                Debug.LogWarning("Index out of range.");
                return;
            }

            for (int y = 0; y < alphamaps.GetLength(1); y++) {
                for (int x = 0; x < alphamaps.GetLength(0); x++) {

                    float a1 = alphamaps[x, y, splatIndex1];
                    alphamaps[x, y, splatIndex1] = alphamaps[x, y, splatIndex2];
                    alphamaps[x, y, splatIndex2] = a1;
                }
            }

            terrainData.SetAlphamaps(0, 0, alphamaps);
        }

        public static void LowerTerrain(Terrain terrain, float amount) {
            TerrainData terrainData = terrain.terrainData;
            float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
            float heightDecrease = amount / terrainData.size.y;

            for (int y = 0; y < heights.GetLength(1); y++) {
                for (int x = 0; x < heights.GetLength(0); x++) {
                    heights[x, y] -= heightDecrease;
                }
            }

            terrainData.SetHeights(0, 0, heights);
        }

        public static void RemoveSplat(Terrain terrain, int splatIndex, int baseSplatIndex) {
            if (splatIndex == baseSplatIndex)
                return;

            TerrainData terrainData = terrain.terrainData;

            float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

            if (splatIndex >= alphamaps.GetLength(2) && baseSplatIndex >= alphamaps.GetLength(2)) {
                Debug.LogWarning("Index out of range.");
                return;
            }

            for (int y = 0; y < alphamaps.GetLength(1); y++) {
                for (int x = 0; x < alphamaps.GetLength(0); x++) {

                    float splatValue = alphamaps[x, y, splatIndex];

                    if (splatValue == 1) {
                        alphamaps[x, y, splatIndex] = 0;
                        alphamaps[x, y, baseSplatIndex] = 1;
                        continue;
                    }


                    float baseMulti = 1 / (1 - splatValue);

                    for (int i = 0; i < alphamaps.GetLength(2); i++) {
                        if (i == splatIndex) {
                            alphamaps[x, y, i] = 0;
                        } else {
                            alphamaps[x, y, i] *= baseMulti;
                        }
                    }
                }
            }

            terrainData.SetAlphamaps(0, 0, alphamaps);
        }


        public static void ColorUnderWaterTerrain(Terrain terrain, float min, float max, int groundIndex, int baseIndex) {
            if (groundIndex == baseIndex)
                return;

            TerrainData terrainData = terrain.terrainData;
            float rMin = min / terrainData.size.y;
            float rMax = max / terrainData.size.y;

            float rDiff = rMax - rMin;

            float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
            float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

            float alphaToHeightIndex = heights.GetLength(0) / alphamaps.GetLength(0);

            if (groundIndex >= alphamaps.GetLength(2)) {
                Debug.LogWarning("Index out of range.");
                return;
            }

            for (int y = 0; y < alphamaps.GetLength(1); y++) {
                for (int x = 0; x < alphamaps.GetLength(0); x++) {
                    float groundValue = 1 - Mathf.Clamp01((heights[(int)(x * alphaToHeightIndex), (int)(y * alphaToHeightIndex)] - rMin) / rDiff);

                    float mul = 1 / (1 - groundValue);

                    for (int i = 0; i < alphamaps.GetLength(2); i++) {
                        alphamaps[x, y, i] *= (1 - groundValue);
                    }
                }
            }

            terrainData.SetAlphamaps(0, 0, alphamaps);
        }
    }
}