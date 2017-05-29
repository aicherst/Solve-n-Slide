﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour {
    public int health = 100, maxHealth = 100;
    //public int charges = 5, maxCharges = 5;
    Jetpack jetpack;
    CharacterMovement characterMovement;
    //public enum Phase {
    //    MANIPULATION_PHASE, ACTION_PHASE
    //}
    //Phase currentPhase = Phase.MANIPULATION_PHASE;

    //Vector3 startPosition;
    //Vector3 manipulationPosition;

    void Awake() {
        jetpack = GetComponent<Jetpack>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    // Use this for initialization
    void Start() {
        //startPosition = transform.position;
        //jetpack.enabled = false;
        //characterMovement.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        //if(Input.GetButtonDown("PhaseSwitch")) {
        //    if (currentPhase == Phase.MANIPULATION_PHASE) {
        //        currentPhase = Phase.ACTION_PHASE;

        //        jetpack.enabled = true;
        //        characterMovement.enabled = true;

        //        transform.position = startPosition;
        //        manipulationPosition = transform.position;

        //        health = maxHealth;
        //        charges = maxCharges;
        //        //TODO: reset jetpack fuel
        //    } else {
        //        currentPhase = Phase.MANIPULATION_PHASE;

        //        jetpack.enabled = false;
        //        characterMovement.enabled = false;

        //        transform.position = manipulationPosition;
        //    }
        //}

        jetpack.Thrust(Input.GetButton("Jetpack"));
        Vector2 movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jetpack.MovementInput(movementAxis);
    }

    public int getHealth() { return health; }
    public int getMaxHealth() { return maxHealth; }

    public float getFuel() { return jetpack.fuel; }
    public float getMaxFuel() { return jetpack.maxFuel; }

    //public int getCharges() { return charges; }
    //public int getMaxCharges() { return maxCharges; }

    //public Phase getCurrentPhase() { return currentPhase; }
}
