using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnClick : MonoBehaviour {
    [SerializeField]
    private string sceneName;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && MouseInput.mouseOver.data == gameObject) {
            SceneManager.LoadScene(sceneName);
        }
	}
}
