using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationCharacter : MonoBehaviour {
    public int charges = 5, maxCharges = 5;
	public int fuelTanks = 3, maxFuelTanks = 3;

	private static Terrain levelTerrain;
	private int xResolution;
	private int zResolution;

	private static string[] unmodifiableTerrainNames = { "GrassHillAlbedoUnmodifiable", "GrassRockyAlbedoUnmodifiable", "sand_colorUnmodifiable" };

	public enum ManipulationPhase {
		CHANGE_TERRAIN, FUELTANK_PLACEMENT
	}
	private static ManipulationPhase currentManipulationPhase = ManipulationPhase.CHANGE_TERRAIN;

	public GameObject terrainMarker;
	public GameObject fuelTankObject;

	public GameObject manipulationSelectorPrefab;
	public static GameObject manipulationSelector;
	public GameObject fuelTankSelectorPrefab;
	public static GameObject fuelTankSelector;

    private bool active;

    void Awake() {
		manipulationSelector = Instantiate(manipulationSelectorPrefab);
		fuelTankSelector = Instantiate(fuelTankSelectorPrefab);
    }

	void Start () {
        levelTerrain = Player.getLevelTerrain();
        xResolution = levelTerrain.terrainData.heightmapWidth;
        zResolution = levelTerrain.terrainData.heightmapHeight;
        TerrainMarker.terrainMarkers = new List<GameObject>();
		FuelTankPickup.fuelTanks = new List<GameObject>();
    }

	void Update () {
        if (active) {
			if (currentManipulationPhase == ManipulationPhase.CHANGE_TERRAIN) {
				RaycastHit hit2;
				Ray ray2 = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
				if (Physics.Raycast(ray2, out hit2)) {
					if (hit2.collider.gameObject.layer == 8) {
						manipulationSelector.transform.position = hit2.point + Vector3.up * 50;
					}
				}
			}
			else if (currentManipulationPhase == ManipulationPhase.FUELTANK_PLACEMENT) {
				fuelTankSelector.transform.position = transform.position + transform.Find("EgoCamera").transform.forward * 5f;
				if (fuelTanks == 0) {
					fuelTankSelector.transform.Find("fueltank").GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.5f);
				}
				else {
					fuelTankSelector.transform.Find("fueltank").GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.5f);
				}
			}

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
                currentManipulationPhase = ManipulationPhase.CHANGE_TERRAIN;
				manipulationSelector.SetActive(true);
				fuelTankSelector.SetActive(false);
            } else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
                currentManipulationPhase = ManipulationPhase.FUELTANK_PLACEMENT;
				manipulationSelector.SetActive(false);
				fuelTankSelector.SetActive(true);
            }

            if (currentManipulationPhase == ManipulationPhase.CHANGE_TERRAIN) {
                //raise terrain
                if (Input.GetMouseButtonDown(0)) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider.gameObject.layer == 8 && charges >= 1) {
                            if (isTerrainModifiable(levelTerrain.terrainData.splatPrototypes[TerrainCharactaristicsReader.GetMainTexture(hit.point)].texture.name)) {
                                charges--;
								Audiocontroller.playRaiseHillSound(gameObject.transform.position);
                                manipulateTerrainArea(hit.point, 25, true);
                                GameObject currentMarker = Instantiate(terrainMarker, hit.point, Quaternion.identity);
                                currentMarker.GetComponent<TerrainMarker>().terrainLowered = false;
                                TerrainMarker.terrainMarkers.Add(currentMarker);
                            }
                        } else if (hit.collider.gameObject.layer == 9 && hit.collider.gameObject.GetComponent<TerrainMarker>().terrainLowered == true) {
                            charges++;
                            manipulateTerrainArea(hit.collider.gameObject.transform.position, 25, hit.collider.gameObject.GetComponent<TerrainMarker>().terrainLowered);
							Audiocontroller.playLowerHillSound(gameObject.transform.position);
                            TerrainMarker.terrainMarkers.Remove(hit.collider.gameObject);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
                //lower terrain
                else if (Input.GetMouseButtonDown(1)) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider.gameObject.layer == 8 && charges >= 1) {
                            if (isTerrainModifiable(levelTerrain.terrainData.splatPrototypes[TerrainCharactaristicsReader.GetMainTexture(hit.point)].texture.name)) {
                                charges--;
								Audiocontroller.playLowerHillSound(gameObject.transform.position);
                                manipulateTerrainArea(hit.point, 25, false);
                                GameObject currentMarker = Instantiate(terrainMarker, hit.point, Quaternion.identity);
                                currentMarker.GetComponent<TerrainMarker>().terrainLowered = true;
                                currentMarker.GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f, 0.5f);
                                TerrainMarker.terrainMarkers.Add(currentMarker);
                            }
                        } else if (hit.collider.gameObject.layer == 9 && hit.collider.gameObject.GetComponent<TerrainMarker>().terrainLowered == false) {
                            charges++;
                            manipulateTerrainArea(hit.collider.gameObject.transform.position, 25, hit.collider.gameObject.GetComponent<TerrainMarker>().terrainLowered);
							Audiocontroller.playRaiseHillSound(gameObject.transform.position);
                            TerrainMarker.terrainMarkers.Remove(hit.collider.gameObject);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            } else if (currentManipulationPhase == ManipulationPhase.FUELTANK_PLACEMENT) {
                //place fueltank
                if (Input.GetMouseButtonDown(0) && fuelTanks >= 1) {
                    fuelTanks--;
					GameObject currentFuelTank = Instantiate(fuelTankObject, transform.position + transform.Find("EgoCamera").transform.forward * 5f, Quaternion.identity);
					Audiocontroller.playFuelTankPlacementSound(gameObject.transform.position);
					FuelTankPickup.fuelTanks.Add(currentFuelTank);
                }
                //remove fueltank
                else if (Input.GetMouseButtonDown(1)) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider.gameObject.layer == 10) {
                            fuelTanks++;
							FuelTankPickup.fuelTanks.Remove(hit.collider.gameObject);
							Destroy(hit.collider.gameObject);
							Audiocontroller.playFuelTankPickupSound(gameObject.transform.position);
                        }
                    }
                }
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

	private bool isTerrainModifiable (string textureName) {
		bool returnValue = true;
		for (int i = 0; i < unmodifiableTerrainNames.Length; i++) {
			if (textureName == unmodifiableTerrainNames[i]) {
				returnValue = false;
			}
		}
		return returnValue;
	}

	public static void changeUnmodifiableTerrainToNormal () {
		float[, ,] alphas = levelTerrain.terrainData.GetAlphamaps(0, 0, levelTerrain.terrainData.alphamapWidth, levelTerrain.terrainData.alphamapHeight);

		for (int i = 0; i < unmodifiableTerrainNames.Length; i++) {
			
			bool isTextureinLevel = false;
			for (int t = 0; t < levelTerrain.terrainData.splatPrototypes.Length; t++) {
				if (unmodifiableTerrainNames[i] == levelTerrain.terrainData.splatPrototypes[t].texture.name) {
					isTextureinLevel = true;
				}
			}

			if (isTextureinLevel == true) {
				int oldTexture = getIndexOfTextureByName(unmodifiableTerrainNames[i]);
				int newTexture = getIndexOfTextureByName(unmodifiableTerrainNames[i].Substring(0, unmodifiableTerrainNames[i].Length - 12)); //-12 kommt wegen dem Wort "Unmodifiable" alle Terrains müssen am Ende dieses Wort haben

				for (int j = 0; j < levelTerrain.terrainData.alphamapWidth; j++) {
					for (int k = 0; k < levelTerrain.terrainData.alphamapHeight; k++) {
						alphas[j, k, newTexture] = Mathf.Max(alphas[j, k, oldTexture], alphas[j, k, newTexture]);
						alphas[j, k, oldTexture] = 0f;
					}
				}
			}
		}
		levelTerrain.terrainData.SetAlphamaps(0, 0, alphas);
	}

	private static int getIndexOfTextureByName (string textureName) {
		for (int i = 0; i < levelTerrain.terrainData.splatPrototypes.Length; i++) {
			if (textureName == levelTerrain.terrainData.splatPrototypes[i].texture.name) {
				return i;
			}
		}
		return -1;
	}

    public int getCharges() { return charges; }
    public int getMaxCharges() { return maxCharges; }
	public int getFuelTanks() { return fuelTanks; }
	public int getMaxFuelTanks() { return maxFuelTanks; }
	public static ManipulationPhase getCurrentManipulationPhase() { return currentManipulationPhase; }
	public static Terrain getLevelTerrain() { return levelTerrain; }

    public void setActive(bool a) {
        active = a;
        GetComponentInChildren<Camera>().enabled = a;
        GetComponentInChildren<AudioListener>().enabled = a;
        GetComponentInChildren<ManipulationCamera>().enabled = a;
        manipulationSelector.SetActive(a);
		fuelTankSelector.SetActive(a);
    }

    public void reset() {
        charges = maxCharges;
        fuelTanks = maxFuelTanks;
    }
}
