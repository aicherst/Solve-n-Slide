using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMarker : MonoBehaviour {
	public bool terrainLowered = false;
	public static List<GameObject> terrainMarkers;

	// Use this for initialization
	void Start () {
		gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, ManipulationCharacter.getLevelTerrain().terrainData.size.y, gameObject.transform.localScale.z);
	}

	public static void activateAllMArkers () {
		foreach (GameObject nextMarker in terrainMarkers) {
			nextMarker.SetActive(true);
		}
	}

	public static void deactivateAllMArkers () {
		foreach (GameObject nextMarker in terrainMarkers) {
			nextMarker.SetActive(false);
		}
	}
}
