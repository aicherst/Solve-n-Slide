using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Lock))]
public class ForceField : MonoBehaviour {
    private Collider[] colliders;

	// Use this for initialization
	void Start () {
        colliders = GetComponentsInChildren<Collider>();

        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
                EnableColliders(true);
                break;
            case GamePhase.Manipulation:
                EnableColliders(false);
                break;
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }

    private void EnableColliders(bool enable) {
        foreach(Collider _collider in colliders) {
            _collider.enabled = enable;
        }
    }

    private void Update() {
        if(!Application.isPlaying) {
            LockType lockType = GetComponent<Lock>().lockType;

            Color color = LockTypeToColor.Convert(lockType);
            color = ClampColor(color);


            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers) {
                meshRenderer.material.color = color;
            }
        }
    }

    private Color ClampColor(Color color) {
        float maxColor = Mathf.Max(color.r, color.g, color.b);
        float cMult = Mathf.Min(maxColor, 0.37f) / maxColor;
        return color * cMult;
    }
}
