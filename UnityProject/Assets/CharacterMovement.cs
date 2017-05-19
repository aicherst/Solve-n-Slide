using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour, ICharacterMovement {
    private CharacterController characterController;
    private Vector3 velocity;

    private bool wasGrounded;

    private Vector2 input;

    public GroudAttributes groudAttributes;
    public Vector2 airSpeed = new Vector2(0.15f, 0.15f);
    public Vector2 floorSpeed = new Vector2(0.05f, 0);

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    // Use this for initialization
    void Start() {
    }

    void FixedUpdate() {
        velocity += Physics.gravity * Time.deltaTime;


        if (characterController.isGrounded) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit)) {
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(velocity, hit.normal);
                velocity = projectedVelocity;
            }

            velocity += transform.TransformDirection(new Vector3(input.x * floorSpeed.x, 0, input.y * floorSpeed.y));
        } else {
            velocity += transform.TransformDirection(new Vector3(input.x * airSpeed.x, 0, input.y * airSpeed.y));
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

    public void MovementInput(Vector2 input) {
        this.input = input; 
    }
}

[Serializable]
public struct GroudAttributes {
    public float friction; // value between 0 (no friction) and 1 (no velocity loss)
}