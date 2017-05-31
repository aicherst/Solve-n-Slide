using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour, ICharacterCollision {
    public int health = 100, maxHealth = 100;
    IJetpack jetpack;

    void Awake() {
        jetpack = GetComponent<IJetpack>();
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

    void OnTriggerEnter(Collider other) {
        //Debug.Log("Trigger: "+other.tag);
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("Collision: "+collision.gameObject.tag);
    }

    public int getHealth() { return health; }
    public int getMaxHealth() { return maxHealth; }

    public float getFuel() { return jetpack.fuel; }
    public float getMaxFuel() { return jetpack.maxFuel; }

    public void collision(float strength) {
        //Debug.Log("collision: " + strength);
    }
}
