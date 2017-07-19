using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {
    private CharacterController characterController;
    private Vector3 _velocity;

    private bool wasGrounded;

    private ICharacter character;

    private ITerrainTextureAttributesReader terrainTextureAttributesReader;

    #region public members
    public Vector3 velocity {
        get {
            return _velocity;
        }
    }

    public bool grounded {
        get {
            return wasGrounded;
        }
    }
    #endregion

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<ICharacter>();

        terrainTextureAttributesReader = FindObjectOfType<TerrainTextureAttributesManager>();
    }

    void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
            case GamePhase.Manipulation:
                _velocity = Vector3.zero;
                break;
        }
    }

    void FixedUpdate() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Action)
            return;


        _velocity += Physics.gravity * Time.deltaTime;

        Terrain terrain = null;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up * characterController.radius, characterController.radius, -Vector3.up, out hit, float.PositiveInfinity, Layers.pickUp.Inverse())) {
            terrain = hit.collider.GetComponent<Terrain>();

            wasGrounded = hit.distance < 0.1f + characterController.radius;

            Vector3 terrainNormal = hit.normal;

            float num = Vector3.Dot(terrainNormal, terrainNormal);
            float direction = Vector3.Dot(_velocity, terrainNormal) / num;

            if (direction < 0 && wasGrounded) {
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(_velocity, terrainNormal);

                Vector3 difference = projectedVelocity - _velocity;
                character.Collision(difference.magnitude);

                _velocity = projectedVelocity;
            }
        } else {
            wasGrounded = false;
        }

        if (wasGrounded) {
            TerrainTextureAttributes terrainCharacteristics = terrainTextureAttributesReader.GetTerrainCharacteristics(terrain, transform.position);

            _velocity *= Mathf.Clamp01(1 - terrainCharacteristics.friction * Time.deltaTime);
        }

        //ApplyGravity();

        characterController.Move(_velocity * Time.deltaTime);
    }

    public void AddVelocity(Vector3 velocity) {
        _velocity += velocity;
    }

    #region debug
    void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, _velocity);
    }
    #endregion
}