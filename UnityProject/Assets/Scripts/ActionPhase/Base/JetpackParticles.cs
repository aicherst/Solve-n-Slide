using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackParticles : MonoBehaviour {
    private Jetpack jetpack;
    private ParticleSystem _particleSystem;

    // Use this for initialization
    void Start() {
        jetpack = GetComponentInParent<Jetpack>();
        _particleSystem = GetComponent<ParticleSystem>();


        if (jetpack == null) {
            Debug.LogWarning("No Jetpack script found");
            enabled = false;
            return;
        }

        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }


    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Manipulation:
                ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
                emissionModule.enabled = false;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Action)
            return;

        ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
        emissionModule.enabled = jetpack.isActivated;
    }
}
