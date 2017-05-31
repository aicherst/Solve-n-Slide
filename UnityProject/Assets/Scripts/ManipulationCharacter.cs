using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationCharacter : MonoBehaviour {
    public int charges = 5, maxCharges = 5;

	private Terrain levelTerrain;
	private int xResolution;
	private int zResolution;


	void Start () {
		levelTerrain = Player.getLevelTerrain();
		xResolution = levelTerrain.terrainData.heightmapWidth;
		zResolution = levelTerrain.terrainData.heightmapHeight;

	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && charges >= 1) {
			charges--;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				manipulateTerrainArea(hit.point, 50, 50, true); 
			}
		}
		if (Input.GetMouseButtonDown(1) && charges >= 1) {
			charges--;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				manipulateTerrainArea(hit.point, 50, 50, false);
			}
		}
	}


	private void manipulateTerrainArea (Vector3 point, int lenx, int lenz, bool raise) {
		int areax;
		int areaz;
		int terX = (int)((point.x / levelTerrain.terrainData.size.x) * xResolution);
		int terZ = (int)((point.z / levelTerrain.terrainData.size.z) * zResolution);
		lenx += 0;
		lenz += 0;
		terX -= (lenx / 2);
		terZ -= (lenz / 2);
		if (terX < 0)
			terX = 0;
		if (terX > xResolution)
			terX = xResolution;
		if (terZ < 0)
			terZ = 0;
		if (terZ > zResolution)
			terZ = zResolution;
		float[,] heights = levelTerrain.terrainData.GetHeights(terX, terZ, lenx, lenz);
		for (areax = 0; areax < lenx; areax++) {
			for (areaz = 0; areaz < lenz; areaz++) {
				if ((areax > -1) && (areaz > -1) && (areax < xResolution) && (areaz < zResolution)) {
					float intensity = 25 + Mathf.Max(Mathf.Abs(areax - 25), Mathf.Abs(areaz - 25));

					if (raise == true) {
						heights[areax, areaz] += 0.25f * Mathf.Pow(0.9f, intensity);
					}
					else {
						heights[areax, areaz] -= 0.25f * Mathf.Pow(0.9f, intensity);
					}
				}
			}
		}
		levelTerrain.terrainData.SetHeights(terX, terZ, heights);
	}

    public int getCharges() { return charges; }
    public int getMaxCharges() { return maxCharges; }
}
