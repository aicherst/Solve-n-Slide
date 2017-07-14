using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableModelDuringActionPhase : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
                foreach (SkinnedMeshRenderer meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
                break;
            case GamePhase.Manipulation:
                foreach (SkinnedMeshRenderer meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                break;
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }
}
