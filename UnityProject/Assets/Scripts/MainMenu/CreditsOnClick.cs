using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsOnClick : MonoBehaviour {
    public GameObject credits;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && MouseInput.mouseOver.data == gameObject) {
            credits.SetActive(true);
            GameStateManager.instance.inputLock.SetData(InputLock.PauseMenu);
        }
    }
}
