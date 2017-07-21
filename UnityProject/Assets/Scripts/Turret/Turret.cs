using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDestroyable {
    public Transform muzzle;
    public Transform head;

    public GameObject projectilePrefab;

    public float cooldownTime = float.PositiveInfinity;
    public float headRotationSpeed = 90;
    public float lockRotationTime = 2;

    public float shootAngleThreshold = 5;

    [SerializeField]
    private float _range = 50;

    private float remainingCooldownTime;
    private float remainingLockRotationTime;

    private Transform target;

    private Quaternion baseHeadRotation;

    private Quaternion initalRotation;

    void Start() {
        CreateTriggerCollider();

        initalRotation = head.rotation;

        baseHeadRotation = Quaternion.Inverse(transform.rotation) * head.rotation;

        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public float range {
        get {
            return _range;
        }
    }

    private void CreateTriggerCollider() {
        CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.height = 1000;
        capsuleCollider.radius = _range;
        capsuleCollider.isTrigger = true;
    }


    // Use this for
    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Manipulation:
                ResetTurret();
                break;
        }
    }

    public void ResetTurret() {
        remainingCooldownTime = 0;
        remainingLockRotationTime = 0;
        target = null;

        head.rotation = initalRotation;
    }

    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Action)
            return;

        remainingCooldownTime -= Time.deltaTime;
        remainingLockRotationTime -= Time.deltaTime;

        if (target == null) {
            return;
        }

        float remainingAngle = RotateTowardsTarget();

        if (remainingCooldownTime <= 0 && remainingAngle < shootAngleThreshold) {
            remainingCooldownTime = cooldownTime;
            remainingLockRotationTime = lockRotationTime;

            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = muzzle.position;
            projectile.transform.rotation = muzzle.rotation;

            IHomingRocket homingRocket = projectile.GetComponent<IHomingRocket>();
            if (homingRocket != null) {
                homingRocket.SetTarget(target);
            }
			AudioManager.PlayTurretShotSound(transform.position);
        }

    }

    private float RotateTowardsTarget() {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        lookRotation = lookRotation * baseHeadRotation;

        if (remainingLockRotationTime <= 0) {
            head.rotation = Quaternion.RotateTowards(head.rotation, lookRotation, Time.deltaTime * headRotationSpeed);
        }


        return Quaternion.Angle(head.rotation, lookRotation);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.player)) {
            target = other.transform;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform == target) {
            target = null;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void SetDestroyed(bool destroyed) {
        gameObject.SetActive(!destroyed);
    }
}
