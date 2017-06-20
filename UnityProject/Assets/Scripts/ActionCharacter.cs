using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour, ICharacter {
    public int health = 100, maxHealth = 100;
    public GameObject finishFirework;
    IJetpack jetpack;
    CharacterMovement characterMovement;
    bool levelFinished, dead;
    private bool active;
    //private Transform startTransform;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Awake() {
        jetpack = GetComponent<IJetpack>();
        characterMovement = GetComponent<CharacterMovement>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (active) {
            jetpack.Thrust(Input.GetButton("Jetpack"));
            Vector2 movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            jetpack.MovementInput(movementAxis);
        }
    }

    void OnTriggerEnter(Collider other) {
        //Debug.Log("Trigger: "+other.tag);
        if(other.tag == "Finish") {
            setActive(false, true);
            levelFinished = true;
            characterMovement.Reset();
            Instantiate(finishFirework, transform.position + transform.forward * 30 + Vector3.down, Quaternion.Euler(-90,0,0));
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
            setActive(false, true);
            characterMovement.Reset();
        }
    }

    public void setActive(bool movement, bool camera) {
        active = movement;
        GetComponentInChildren<Camera>().enabled = camera;
        GetComponentInChildren<AudioListener>().enabled = camera;
        GetComponentInChildren<EgoCamera>().enabled = camera;
        GetComponentInChildren<CharacterController>().enabled = movement;
        GetComponentInChildren<CharacterMovement>().enabled = movement;
        GetComponentInChildren<Jetpack>().enabled = movement;
    }

    public void reset() {
        health = maxHealth;
        jetpack.fuel = jetpack.maxFuel;
        transform.position = startPosition;
        transform.rotation = startRotation;
        dead = false;
        characterMovement.Reset();
    }
}
