using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public int health = 100, maxHealth = 100;
    public int charges = 3, maxCharges = 5;
    IJetpack jetpack;
    ICharacterMovement characterMovement;

    void Awake() {
        jetpack = GetComponent<IJetpack>();
        characterMovement = GetComponent<ICharacterMovement>();
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        jetpack.Thrust(Input.GetButton("Jetpack"));
        Vector2 movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        characterMovement.MovementInput(movementAxis);
    }

    public int getHealth() { return health; }
    public int getMaxHealth() { return maxHealth; }

    public float getFuel() { return jetpack.fuel; }
    public float getMaxFuel() { return jetpack.maxFuel; }

    public int getCharges() { return charges; }
    public int getMaxCharges() { return maxCharges; }
}
