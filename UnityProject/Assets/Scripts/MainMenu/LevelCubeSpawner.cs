using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCubeSpawner : MonoBehaviour {
    public GameObject levelCubePrefab;

    public float radius = 5;

    public float width = 2;
    public float spaceing = 0.3f;

    public LevelCubeData[] levelCubesData;

    // Use this for initialization
    void Start () {
        float u = 2 * Mathf.PI * radius;
        int amount = (int)(u / (width + spaceing));
        float angle = 360f / amount;

        for(int i = 0; i < levelCubesData.Length; i++) {
            GameObject gLevelCube = Instantiate(levelCubePrefab);
            gLevelCube.transform.position = transform.position + Quaternion.Euler(0, angle * i + transform.localEulerAngles.y, 0) * Vector3.forward * radius;
            gLevelCube.transform.LookAt(transform.position);

            LevelCube levelCube = gLevelCube.GetComponent<LevelCube>();
            levelCube.SetPreviewTexture(levelCubesData[i].preview);
            levelCube.levelName = levelCubesData[i].levelName;
        }
    }

    private void Update() {
        if (GameStateManager.instance.inputLock.data == InputLock.PauseMenu)
            return;

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(MouseInput.CameraRay(Camera.main), out hit)) {
                LevelCube levelCube = hit.collider.GetComponent<LevelCube>();

                if(levelCube != null) {
                    SceneManager.LoadScene(levelCube.levelName);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

        float u = 2 * Mathf.PI * radius;
        int amount = (int) (u / (width + spaceing));
        float angle = 360f / amount;

        Gizmos.color = Color.green;

        for(int i = 0; i < amount; i++) {

            if (i == 1)
                Gizmos.color = Color.white;

            Gizmos.DrawWireCube(transform.position + Quaternion.Euler(0, angle * i + transform.localEulerAngles.y, 0) * Vector3.forward * radius, new Vector3(width, width, 0.1f));
        }
    }

    [System.Serializable]
    public struct LevelCubeData {
        public Texture2D preview;
        public string levelName;
    }
}
