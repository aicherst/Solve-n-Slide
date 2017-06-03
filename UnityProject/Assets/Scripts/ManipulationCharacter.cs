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
		//raise terrain
		if (Input.GetMouseButtonDown(0) && charges >= 1) {
			charges--;
			RaycastHit hit;
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
			if (Physics.Raycast(ray, out hit)) {
				manipulateTerrainArea(hit.point, 25, true); 
			}
		}
		//lower terrain
		if (Input.GetMouseButtonDown(1) && charges >= 1) {
			charges--;
			RaycastHit hit;
			Ray ray = Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0f));
			if (Physics.Raycast(ray, out hit)) {
				manipulateTerrainArea(hit.point, 25, false);
			}
		}
	}


	private void manipulateTerrainArea (Vector3 point, int r, bool raise) {
		int areax;
		int areaz;
		int terX = (int)((point.x / levelTerrain.terrainData.size.x) * xResolution);
		int terZ = (int)((point.z / levelTerrain.terrainData.size.z) * zResolution);
		terX -= r;
		terZ -= r;
		if (terX < 0)
			terX = 0;
		if (terX > xResolution)
			terX = xResolution;
		if (terZ < 0)
			terZ = 0;
		if (terZ > zResolution)
			terZ = zResolution;
		float[,] heights = levelTerrain.terrainData.GetHeights(terX, terZ, (r*2), (r*2));

		for (areax = -r; areax < r; areax++) {
			for (areaz = -r; areaz < r; areaz++) {
				int y = (r * r) - ((areax * areax) + (areaz * areaz));
				if (y > 0 && areax < xResolution && areaz < zResolution) {
					if (raise == true) {
						heights[areax + r, areaz + r] += (float)y / (150f * levelTerrain.terrainData.size.y);
					}
					else {
						heights[areax + r, areaz + r] -= (float)y / (150f * levelTerrain.terrainData.size.y);
					}
				}
			}
		}

		levelTerrain.terrainData.SetHeights(terX, terZ, heights);
	}

    public int getCharges() { return charges; }
    public int getMaxCharges() { return maxCharges; }
}
