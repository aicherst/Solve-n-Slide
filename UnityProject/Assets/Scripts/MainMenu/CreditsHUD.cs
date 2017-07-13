using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsHUD : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown) {
            gameObject.SetActive(false);
            GameStateManager.instance.inputLock.SetData(InputLock.Game);
        }
    }
}
