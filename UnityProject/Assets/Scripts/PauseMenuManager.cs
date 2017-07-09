using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (Input.GetKeyDown("escape")) {
            switch (gameStateManager.inputLock.data) {
                case InputLock.Game:
                    gameStateManager.inputLock.SetData(InputLock.PauseMenu);
                    Time.timeScale = 0;

                    break;
                case InputLock.PauseMenu:
                    gameStateManager.inputLock.SetData(InputLock.Game);
                    Time.timeScale = 1;

                    break;
            }
        }
    }
}
