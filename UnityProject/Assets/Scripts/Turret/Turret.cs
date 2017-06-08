using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public Transform muzzle;
    public Transform head;

    public GameObject projectilePrefab;

    public float cooldownTime = float.PositiveInfinity;
    public float headRotationSpeed = 90;
    public float lockRotationTime = 2;

    public float shootAngleThreshold = 5;

    private float remainingCooldownTime;
    private float remainingLockRotationTime;

    private Transform target;

    private Quaternion baseHeadRotation;

    private void Start() {
        baseHeadRotation = Quaternion.Inverse(transform.rotation) * head.rotation;
    }

    private void Update() {
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

    private void OnTriggerEnter(Collider other) {
        if(other.tag == Tags.player) {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform == target) {
            target = null;
        }
    }
}
