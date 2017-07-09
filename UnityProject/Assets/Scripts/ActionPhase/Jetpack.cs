using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class Jetpack : MonoBehaviour {
    [SerializeField]
    private float strength = 0.25f;

    [SerializeField]
    private float _maxFuel = 100;

    [SerializeField]
    private float fuelConsumptionRate = 20;


    private float _fuel;
    private Vector2 input;

    public Vector2 airSpeed = new Vector2(0.15f, 0.15f);
    public Vector2 floorSpeed = new Vector2(0.05f, 0);

    private CharacterMovement characterMovement;

    void Awake() {
        characterMovement = GetComponent<CharacterMovement>();
    }

    void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
                _fuel = _maxFuel;
                break;
        }
    }


    // Update is called once per frame
    void FixedUpdate() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Action)
            return;

        Vector3 velocity = Vector3.zero;

        if (isActivated) {
            velocity += Vector3.up * strength;
            _fuel = Mathf.Max(_fuel - fuelConsumptionRate * Time.fixedDeltaTime, 0);
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (characterMovement.grounded) {
            velocity += transform.TransformDirection(new Vector3(input.x * floorSpeed.x, 0, input.y * floorSpeed.y));
        } else {
            velocity += transform.TransformDirection(new Vector3(input.x * airSpeed.x, 0, input.y * airSpeed.y));
        }

        characterMovement.AddVelocity(velocity);
    }

    public float maxFuel {
        get {
            return _maxFuel;
        }
    }

    public float fuel {
        get {
            return _fuel;
        }
        set {
            _fuel = Mathf.Clamp(value, 0, _maxFuel);
        }
    }

    public float fuelPercentage {
        get {
            return _fuel / _maxFuel;
        }
    }

    public bool isActivated {
        get {
            return Input.GetButton("Jetpack") && _fuel > 0;
        }
    }
}
