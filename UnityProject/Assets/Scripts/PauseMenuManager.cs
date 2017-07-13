using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour {
    private void Start() {
        GameStateManager.instance.inputLock.AddListener(OnInputLockChange);
    }

    // Use this for
    public void OnInputLockChange(ReadOnlyProperty<InputLock> changedProperty, InputLock newData, InputLock oldData) {
       switch(newData) {
            case InputLock.Game:
                Time.timeScale = 1;
                break;
            case InputLock.PauseMenu:
                Time.timeScale = 0;
                break;
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.inputLock.RemoveListener(OnInputLockChange);
    }

    // Update is called once per frame
    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (Input.GetKeyDown("escape")) {
            switch (gameStateManager.inputLock.data) {
                case InputLock.Game:
                    gameStateManager.inputLock.SetData(InputLock.PauseMenu);
                    break;
                case InputLock.PauseMenu:
                    gameStateManager.inputLock.SetData(InputLock.Game);
                    break;
            }
        }
    }
}
