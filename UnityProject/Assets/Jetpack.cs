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

    private CharacterMovement slideMovement;

    private bool thrustOn;


    void Awake() {
        slideMovement = GetComponent<CharacterMovement>();
    }

    private void Start() {
        _fuel = _maxFuel;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(thrustOn && _fuel > 0) {
            slideMovement.AddVelocity(Vector3.up * strength);
            _fuel = Mathf.Max(_fuel - fuelConsumptionRate * Time.fixedDeltaTime, 0);
        }
    }

    public void Thrust(bool enable) {
        thrustOn = enable;
    }


    public float maxFuel {
        get {
            return maxFuel;
        }
    }

    public float fuel {
        get {
            return _fuel;
        }
        set {
            _fuel = Mathf.Clamp(value, 0, maxFuel);
        }
    }
}
