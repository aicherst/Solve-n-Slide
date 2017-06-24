using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public enum Phase {
        MANIPULATION_PHASE, ACTION_PHASE
    }
    Phase currentPhase = Phase.MANIPULATION_PHASE;

    public GameObject actionCharacterPrefab;
    private ActionCharacter actionCharacterInstance;
    public GameObject manipulationCharacterPrefab;
    private ManipulationCharacter manipulationCharacterInstance;
	
	//for the terrain
	private static Terrain levelTerrain;
	private int alphaMapWidth;
	private int alphaMapHeight;
	private float[,] heightMapBackup;
	private float[,,] alphaMapBackup;

    void Awake() {
        levelTerrain = Terrain.activeTerrain;

        alphaMapWidth = levelTerrain.terrainData.alphamapWidth;
        alphaMapHeight = levelTerrain.terrainData.alphamapHeight;

        heightMapBackup = levelTerrain.terrainData.GetHeights(0, 0, levelTerrain.terrainData.heightmapWidth, levelTerrain.terrainData.heightmapHeight);
        alphaMapBackup = levelTerrain.terrainData.GetAlphamaps(0, 0, alphaMapWidth, alphaMapHeight);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        actionCharacterInstance = FindObjectOfType<ActionCharacter>();
        if(actionCharacterInstance == null)
            actionCharacterInstance = Instantiate(actionCharacterPrefab, transform.position, transform.rotation).GetComponent<ActionCharacter>();
        actionCharacterInstance.setActive(false, false);

        manipulationCharacterInstance = FindObjectOfType<ManipulationCharacter>();
        if(manipulationCharacterInstance == null)
            manipulationCharacterInstance = Instantiate(manipulationCharacterPrefab, transform.position, transform.rotation).GetComponent<ManipulationCharacter>();
        manipulationCharacterInstance.setActive(true);
    }

        // Use this for initialization
    void Start () {		
		//for terrain
	}
	
	void OnApplicationQuit () {
		resetTerrain();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("PhaseSwitch")) {
			if (currentPhase == Phase.MANIPULATION_PHASE) {
				ManipulationCharacter.changeUnmodifiableTerrainToNormal();
				TerrainMarker.deactivateAllMArkers();
                currentPhase = Phase.ACTION_PHASE;
                manipulationCharacterInstance.setActive(false);
				actionCharacterInstance.setActive(true, true);
				Audiocontroller.windSound.Play();
                FuelTankPickup.changeAllFuelTanksLayersForActionPhase();

                RiverSimulation.TerrainRiver[] terrainRivers = FindObjectsOfType<RiverSimulation.TerrainRiver>();
                foreach(RiverSimulation.TerrainRiver terrainRiver in terrainRivers) {
                    terrainRiver.DestroyIntersectingDestroyables();
                }
            } else {
                if (getActionCharacter().getLevelFinished()) {
					resetTerrain();
                    SceneManager.LoadScene(0);
                } else if (getActionCharacter().getDead()) {
                    actionCharacterInstance.setActive(true, true);
                    actionCharacterInstance.reset();
                    FuelTankPickup.activateAllFuelTanks();
                    KeysAndDoors.activateAllKeysAndDoors();
                    Time.timeScale = 1;
				} else {
					levelTerrain.terrainData.SetAlphamaps(0, 0, alphaMapBackup);
					TerrainMarker.activateAllMArkers();
					currentPhase = Phase.MANIPULATION_PHASE;
                    actionCharacterInstance.setActive(false, false);
                    actionCharacterInstance.reset();
					manipulationCharacterInstance.setActive(true);
					FuelTankPickup.activateAllFuelTanks();
					Audiocontroller.windSound.Stop();
                    FuelTankPickup.changeAllFuelTanksLayersForManipulationPhase();
					KeysAndDoors.activateAllKeysAndDoors();
                    SimpleHomingRocket[] rockets = FindObjectsOfType<SimpleHomingRocket>();
                    for(int i = 0; i < rockets.Length; i++) {
                        Destroy(rockets[i].gameObject);
                    }
                    RiverSimulation.TerrainRiver[] terrainRivers = FindObjectsOfType<RiverSimulation.TerrainRiver>();
                    foreach (RiverSimulation.TerrainRiver terrainRiver in terrainRivers) {
                        terrainRiver.ResetIntersectingDestroyables();
                    }
                }
            }
        }
        if (Input.GetKey("escape")) {
            resetTerrain();
            SceneManager.LoadScene(0);
        }
        if(Input.GetButtonDown("Reload")) {
            if(currentPhase == Phase.MANIPULATION_PHASE) {
                resetTerrain();
                manipulationCharacterInstance.reset();
            }
        }
    }

	private void resetTerrain() {
        //TODO: reset terrain, pillars and fueltanks
        levelTerrain.terrainData.SetHeights(0, 0, heightMapBackup);
		levelTerrain.terrainData.SetAlphamaps(0, 0, alphaMapBackup);
		TerrainMarker.resetAllMarkers();
		FuelTankPickup.resetAllFuelTanks();
	}

    public Phase getCurrentPhase() { return currentPhase; }
    public ActionCharacter getActionCharacter() { return actionCharacterInstance.GetComponent<ActionCharacter>(); }
    public ManipulationCharacter getManipulationCharacter() { return manipulationCharacterInstance.GetComponent<ManipulationCharacter>(); }
	public static Terrain getLevelTerrain () {	return levelTerrain; }
}
