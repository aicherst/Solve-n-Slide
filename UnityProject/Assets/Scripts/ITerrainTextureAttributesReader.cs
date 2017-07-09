using UnityEngine;

public interface ITerrainTextureAttributesReader {
	TerrainTextureAttributes GetTerrainCharacteristics (Terrain terrain, Vector3 position);
}
