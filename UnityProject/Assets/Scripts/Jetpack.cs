using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class Jetpack : MonoBehaviour, IJetpack {
    public float strength = 0.25f;
    public float _maxFuel = 100;
    public float fuelConsumptionRate = 10;

    private float _fuel;

    public Vector2 airSpeed = new Vector2(0.15f, 0.15f);
    public Vector2 floorSpeed = new Vector2(0.05f, 0);

    private CharacterMovement characterMovement;

    private bool thrustOn;


    void Awake() {
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Start() {
        _fuel = _maxFuel;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(thrustOn && _fuel > 0) {
            characterMovement.AddVelocity(Vector3.up * strength);
            _fuel = Mathf.Max(_fuel - fuelConsumptionRate * Time.fixedDeltaTime, 0);
        }
    }

    public void MovementInput(Vector2 input) {
        Vector3 velocity;

        if(characterMovement.grounded) {
            velocity = transform.TransformDirection(new Vector3(input.x * floorSpeed.x, 0, input.y * floorSpeed.y));
        } else {
            velocity = transform.TransformDirection(new Vector3(input.x * airSpeed.x, 0, input.y * airSpeed.y));
        }

        characterMovement.AddVelocity(velocity);
    }

    public void Thrust(bool enable) {
        thrustOn = enable;
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
}
