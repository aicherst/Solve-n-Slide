using UnityEngine;

public class TerrainCharactaristicsReader : ITerrainCharactaristicsReader {
	
	private static Terrain terrain;

	void Start () {
		terrain = ManipulationCharacter.getLevelTerrain();
	}

	private static float[] GetTextureMix (Vector3 position) {
		int mapX = (int)(((position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth);
		int mapZ = (int)(((position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight);
         
		float[,,] splatmapData = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        
		float[] cellMix = new float[ splatmapData.GetUpperBound(2) + 1 ];
         
		for (int n = 0; n < cellMix.Length; n++) {
			cellMix[n] = splatmapData[0, 0, n];
		}
		return cellMix;
	}

	public static int GetMainTexture (Vector3 position) {
		float[] mix = GetTextureMix(position);
         
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

	public override TerrainCharacteristics GetTerrainCharacteristics (Vector3 position) {
		return new TerrainCharacteristics();
	}
}
