using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableChildrenDuringActionPhase : MonoBehaviour {
	// Use this for initialization
	void Start () {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Manipulation:
                EnableChildren(true);
                break;
            default:
                EnableChildren(false);
                break;
        }
    }

    private void EnableChildren(bool enable) {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(enable);
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }
}
