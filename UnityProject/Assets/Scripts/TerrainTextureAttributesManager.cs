using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureAttributesManager : MonoBehaviour, ITerrainTextureAttributesReader {
    public TerrainTextureAttributes baseTerrainTextureAttributes;
    public TerrainTextureAttributes[] terrainTexturesAttributes;

    public TerrainTextureAttributes GetTerrainCharacteristics(Terrain terrain, Vector3 position) {
        if (terrain == null)
            return baseTerrainTextureAttributes;

        string textureName = terrain.terrainData.splatPrototypes[GetMainTexture(terrain, position)].texture.name;

        foreach (TerrainTextureAttributes terrainTextureAttributes in terrainTexturesAttributes) {
            if (terrainTextureAttributes.name.Equals(textureName))
                return terrainTextureAttributes;
        }

        return baseTerrainTextureAttributes;
    }

    private static float[] GetTextureMix(Terrain terrain, Vector3 position) {
        TerrainData terrainData = terrain.terrainData;

        int mapX = (int)(((position.x - terrain.transform.position.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((position.z - terrain.transform.position.z) / terrainData.size.z) * terrainData.alphamapHeight);

        float[,,] splatmapData = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

        for (int n = 0; n < cellMix.Length; n++) {
            cellMix[n] = splatmapData[0, 0, n];
        }
        return cellMix;
    }

    public static int GetMainTexture(Terrain terrain, Vector3 position) {
        float[] mix = GetTextureMix(terrain, position);

        float maxMix = 0;
        int maxIndex = 0;

        for (int n = 0; n < mix.Length; n++) {
            if (mix[n] > maxMix) {
                maxIndex = n;
                maxMix = mix[n];
            }
        }
        return maxIndex;
    }
}

[System.Serializable]
public struct TerrainTextureAttributes {
    public string name;
    public float friction;
}
