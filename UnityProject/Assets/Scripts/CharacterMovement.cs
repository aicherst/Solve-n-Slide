using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {
    private CharacterController characterController;
    private Vector3 _velocity;

    public GroudAttributes groudAttributes;

    private bool wasGrounded;

    private ICharacter character;

    private ITerrainCharactaristicsReader terrainCharactaristicsReader;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<ICharacter>();
        terrainCharactaristicsReader = GetComponent<ITerrainCharactaristicsReader>();
    }

    private Vector3 terrainNormal;

    void FixedUpdate() {
        _velocity += Physics.gravity * Time.deltaTime;


        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up * characterController.radius, characterController.radius, -Vector3.up, out hit)) {
            wasGrounded = hit.distance < 0.1f + characterController.radius;

            terrainNormal = hit.normal;

            float num = Vector3.Dot(terrainNormal, terrainNormal);
            float direction = Vector3.Dot(_velocity, terrainNormal) / num;

            if (direction < 0 && wasGrounded) {
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(_velocity, terrainNormal);

                Vector3 difference = projectedVelocity - _velocity;
                character.collision(difference.magnitude);

                _velocity = projectedVelocity;
            }
        } else {
            wasGrounded = false;
        }

        if(wasGrounded) {
            ITerrainCharactaristicsReader.TerrainCharacteristics terrainCharacteristics = terrainCharactaristicsReader.GetTerrainCharacteristics(transform.position);

            _velocity *= Mathf.Clamp01(1 - terrainCharacteristics.friction*Time.deltaTime);
        }

        //ApplyGravity();
    }

    public void Reset() {
        _velocity = Vector3.zero;
    }

    public Vector3 velocity {
        get {
            return _velocity;
        }
    }

    private void ApplyGravity() {
        if(grounded) {
            _velocity += Vector3.ProjectOnPlane(Physics.gravity * Time.deltaTime, terrainNormal);
        } else {
            _velocity += Physics.gravity * Time.deltaTime;
        }
    }

    public bool grounded {
        get {
            return wasGrounded;
        }
    }

    public void AddVelocity(Vector3 velocity) {
        _velocity += velocity;
    }

    // Update is called once per frame
    void Update() {
        characterController.Move(_velocity * Time.deltaTime);
    }

    void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, _velocity);
    }

}

[Serializable]
public struct GroudAttributes {
    public float friction; // value between 0 (no friction) and 1 (no velocity loss)
}