using UnityEngine;

public abstract class ITerrainCharactaristicsReader {

	public struct TerrainCharacteristics {
		public float friction;

		public TerrainCharacteristics (float friction) {
			this.friction = friction;
		}
	}

	public abstract TerrainCharacteristics GetTerrainCharacteristics (Vector3 position);
}
