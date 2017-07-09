using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public GameObject actionCharacterPrefab;

    public Transform spawnPoint;

    private GameObject gActionCharacter;

    public GameObject InstantiateCharacter() {
        gActionCharacter = Instantiate(actionCharacterPrefab);

        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);

        return gActionCharacter;
    }

    // Use this for
    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
                gActionCharacter.transform.position = spawnPoint.position;
                gActionCharacter.transform.rotation = spawnPoint.rotation;
                break;
        }
    }

    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Manipulation)
            return;

        if(gActionCharacter != null) {
            gActionCharacter.transform.position = spawnPoint.position;
            gActionCharacter.transform.rotation = spawnPoint.rotation;
        }
    }
}
