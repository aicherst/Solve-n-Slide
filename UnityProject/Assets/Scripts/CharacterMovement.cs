using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {
    private CharacterController characterController;
    private Vector3 velocity;

    public GroudAttributes groudAttributes;

    private bool wasGrounded;


    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }


    // Use this for initialization
    void Start() {
    }

    private Vector3 terrainNormal;

    void FixedUpdate() {

        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up * characterController.radius, characterController.radius, -Vector3.up, out hit)) {
            bool newGrounded = hit.distance < 0.1f + characterController.radius;

            terrainNormal = hit.normal;

            if (!wasGrounded && newGrounded) {
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(velocity, terrainNormal);
                velocity = projectedVelocity;
            }

            wasGrounded = newGrounded;
        } else {
            wasGrounded = false;
        }

        ApplyGravity();
    }

    private void ApplyGravity() {
        if(grounded) {
            velocity += Vector3.ProjectOnPlane(Physics.gravity * Time.deltaTime, terrainNormal);
        } else {
            velocity += Physics.gravity * Time.deltaTime;
        }
    }

    public bool grounded {
        get {
            return wasGrounded;
        }
    }

    public void AddVelocity(Vector3 velocity) {
        this.velocity += velocity;
    }

    // Update is called once per frame
    void Update() {
        characterController.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, velocity);
    }

}

[Serializable]
public struct GroudAttributes {
    public float friction; // value between 0 (no friction) and 1 (no velocity loss)
}