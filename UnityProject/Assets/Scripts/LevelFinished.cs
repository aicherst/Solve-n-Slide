using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinished : MonoBehaviour {
    public GameObject finishFireworkPrefab;

    void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Finished:
                Instantiate(finishFireworkPrefab, transform.position + transform.forward * 30 + Vector3.down, Quaternion.Euler(-90, 0, 0));
                break;
        }
    }
}
