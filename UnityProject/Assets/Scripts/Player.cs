using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public enum Phase {
        MANIPULATION_PHASE, ACTION_PHASE
    }
    Phase currentPhase = Phase.MANIPULATION_PHASE;

    Vector3 startPosition;
    Vector3 manipulationPosition;

    public GameObject actionCharacterPrefab;
    private GameObject actionCharacterInstance;
    public GameObject manipulationCharacterPrefab;
    private GameObject manipulationCharacterInstance;
	
	//for the terrain
	private static Terrain levelTerrain;
	private int alphaMapWidth;
	private int alphaMapHeight;
	private float[,] heightMapBackup;
	private float[,,] alphaMapBackup;

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        manipulationCharacterInstance = Instantiate(manipulationCharacterPrefab, startPosition, Quaternion.identity);
		
		//for terrain
		levelTerrain = Terrain.activeTerrain;

		alphaMapWidth = levelTerrain.terrainData.alphamapWidth;
		alphaMapHeight = levelTerrain.terrainData.alphamapHeight;

		heightMapBackup = levelTerrain.terrainData.GetHeights(0, 0, levelTerrain.terrainData.heightmapWidth, levelTerrain.terrainData.heightmapHeight);
		alphaMapBackup = levelTerrain.terrainData.GetAlphamaps(0, 0, alphaMapWidth, alphaMapHeight);
	}
	
	void OnApplicationQuit () {
		levelTerrain.terrainData.SetHeights(0, 0, heightMapBackup);
		levelTerrain.terrainData.SetAlphamaps(0, 0, alphaMapBackup);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("PhaseSwitch")) {
            if (currentPhase == Phase.MANIPULATION_PHASE) {
                currentPhase = Phase.ACTION_PHASE;
                manipulationPosition = manipulationCharacterInstance.transform.position;
                Destroy(manipulationCharacterInstance);
                actionCharacterInstance = Instantiate(actionCharacterPrefab, startPosition, Quaternion.identity);
            } else {
                if (getActionCharacter().getLevelFinished()) {
                    SceneManager.LoadScene(0);
                } else if (getActionCharacter().getDead()) {
                    SceneManager.LoadScene(0);
                } else {
                    currentPhase = Phase.MANIPULATION_PHASE;
                    Destroy(actionCharacterInstance);
                    manipulationCharacterInstance = Instantiate(manipulationCharacterPrefab, manipulationPosition, Quaternion.identity);
                }
            }
        }
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
    }

    public Phase getCurrentPhase() { return currentPhase; }
    public ActionCharacter getActionCharacter() { return actionCharacterInstance.GetComponent<ActionCharacter>(); }
    public ManipulationCharacter getManipulationCharacter() { return manipulationCharacterInstance.GetComponent<ManipulationCharacter>(); }
	public static Terrain getLevelTerrain () {	return levelTerrain; }
}
