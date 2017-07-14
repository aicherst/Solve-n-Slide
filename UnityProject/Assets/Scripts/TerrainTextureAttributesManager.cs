using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureAttributesManager : MonoBehaviour, ITerrainTextureAttributesReader {
    public TerrainTextureAttributes baseTerrainTextureAttributes;
    public TerrainTextureAttributes[] terrainTexturesAttributes;

    public TerrainTextureAttributes GetTerrainCharacteristics(Terrain terrain, Vector3 position) {
        if (terrain == null)
            return baseTerrainTextureAttributes;

        string textureName = TerrainHelpers.GetMainTextureName(terrain, position);

        foreach (TerrainTextureAttributes terrainTextureAttributes in terrainTexturesAttributes) {
            if (terrainTextureAttributes.name.Equals(textureName))
                return terrainTextureAttributes;
        }

        return baseTerrainTextureAttributes;
    }
}

[System.Serializable]
public struct TerrainTextureAttributes {
    public string name;
    public float friction;
}
