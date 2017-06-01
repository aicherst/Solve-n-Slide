using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour, ICharacter {
    public int health = 100, maxHealth = 100;
    IJetpack jetpack;
    bool levelFinished, dead;

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
        if(other.tag == "Finish") {
            Time.timeScale = 0;
            levelFinished = true;
        }

    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("Collision: "+collision.gameObject.tag);
    }

    public int getHealth() { return health; }
    public int getMaxHealth() { return maxHealth; }

    public float getFuel() { return jetpack.fuel; }
    public float getMaxFuel() { return jetpack.maxFuel; }

    public bool getLevelFinished() { return levelFinished; }
    public bool getDead() { return dead; }

    public void collision(float strength) {
        if (strength > 25) {
            //Debug.Log("collision: " + strength);
            reduceHealth(10);
        }
        if (strength > 35) {
            //Debug.Log("collision: " + strength);
            reduceHealth(25);
        }
    }

    public void damage(float amount) {
        //Debug.Log("Damage: " + amount);
        reduceHealth((int)amount);
    }

    public void addForce(Vector3 force) {
        //Debug.Log("Add force");
    }

    private void reduceHealth(int amount) {
        health -= amount;
        if(health <= 0) {
            health = 0;
            dead = true;
            Time.timeScale = 0;
        }
    }
}
