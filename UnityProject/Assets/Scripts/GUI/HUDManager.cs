using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    public GameObject loseHUD;
    public GameObject winHUD;
    public GameObject actionHUD;
    public GameObject manipulationHUD;

    public GameObject pauseHUD;

    // Use this for initialization
    void Start () {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
        GameStateManager.instance.inputLock.AddListener(OnInputLockChange);
    }

    // Use this for
    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        manipulationHUD.SetActive(false);
        actionHUD.SetActive(false);
        loseHUD.SetActive(false);
        winHUD.SetActive(false);

        switch (newData) {
            case GamePhase.Finished:
                winHUD.SetActive(true);
                break;
            case GamePhase.Action:
                actionHUD.SetActive(true);
                break;
            case GamePhase.Dead:
                loseHUD.SetActive(true);
                break;
            case GamePhase.Manipulation:
                manipulationHUD.SetActive(true);
                break;
        }
    }

    // Use this for
    public void OnInputLockChange(ReadOnlyProperty<InputLock> changedProperty, InputLock newData, InputLock oldData) {
        switch (newData) {
            case InputLock.PauseMenu:
                pauseHUD.SetActive(true);
                break;
            default:
                pauseHUD.SetActive(false);
                break;
        }
    }
}
