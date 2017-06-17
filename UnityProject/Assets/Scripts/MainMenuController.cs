using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public GameObject optionsCube, exitCube, level1Cube, level2Cube, level3Cube, level4Cube;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        bool mousePressed = Input.GetMouseButtonDown(0);
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject == optionsCube) {
                if (mousePressed)
                    Debug.Log("Options");
                optionsCube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 255);
            } else {
                optionsCube.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 255);
            }

            if (hit.collider.gameObject == exitCube) {
                if (mousePressed)
                    Application.Quit();
                exitCube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 255);
            } else {
                exitCube.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 255);
            }

            if (hit.collider.gameObject == level1Cube) {
                if (mousePressed)
                    SceneManager.LoadScene(1);
                level1Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 255);
            } else {
                level1Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 255);
            }

            if (hit.collider.gameObject == level2Cube) {
                if (mousePressed)
                    SceneManager.LoadScene(2);
                level2Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 255);
            } else {
                level2Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 255);
            }

            if (hit.collider.gameObject == level3Cube) {
                if (mousePressed)
                    Debug.Log("Level3");
                level3Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 255);
            } else {
                level3Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 255);
            }

            if (hit.collider.gameObject == level4Cube) {
                if (mousePressed)
                    Debug.Log("Level4");
                level4Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 255);
            } else {
                level4Cube.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 255);
            }
        }
    }
}
