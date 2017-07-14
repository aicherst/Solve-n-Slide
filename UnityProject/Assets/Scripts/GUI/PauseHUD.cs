using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHUD : MonoBehaviour {

    public void OnResume() {
        GameStateManager.instance.inputLock.SetData(InputLock.Game);
    }

    public void OnMainMenu() {
        GameStateManager.instance.inputLock.SetData(InputLock.Game);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuit() {
        Application.Quit();
    }
}
