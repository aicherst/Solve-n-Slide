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
    private float jetpackConsumptionRate = 20;

    [SerializeField]
    private float moveConsumptionRate = 10;


    private float _fuel;
    private Vector2 input;

    private float acceleration = 10f;

    private CharacterMovement characterMovement;

    private float timeSinceLastUse;

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

        timeSinceLastUse += Time.deltaTime;

        Vector3 velocity = Vector3.zero;

        float fuelConsumed = 0;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_fuel > 0 && (input.x != 0 || input.y != 0)) {
            velocity += transform.TransformDirection(new Vector3(input.x, 0, input.y).normalized * acceleration * Time.deltaTime);
            fuelConsumed = moveConsumptionRate;
            timeSinceLastUse = 0;
        }

        if (isActivated) {
            velocity += Vector3.up * strength;
            fuelConsumed = jetpackConsumptionRate;
            timeSinceLastUse = 0;
        }

        if(timeSinceLastUse > 2 && _fuel < 30) {
            fuel += Time.deltaTime * 10;
        }

        fuel -= fuelConsumed * Time.deltaTime;

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
