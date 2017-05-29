using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour {
    public int health = 100, maxHealth = 100;
    Jetpack jetpack;
    CharacterMovement characterMovement;

    void Awake() {
        jetpack = GetComponent<Jetpack>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        jetpack.Thrust(Input.GetButton("Jetpack"));
        Vector2 movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jetpack.MovementInput(movementAxis);
    }

    public int getHealth() { return health; }
    public int getMaxHealth() { return maxHealth; }

    public float getFuel() { return jetpack.fuel; }
    public float getMaxFuel() { return jetpack.maxFuel; }
}
