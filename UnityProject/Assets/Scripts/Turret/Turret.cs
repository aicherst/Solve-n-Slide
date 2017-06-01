using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public Transform muzzle;

    public GameObject projectilePrefab;

    public float cooldown = float.PositiveInfinity;

    private float remainingTime;

    private void Update() {
        remainingTime -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == Tags.player && remainingTime <= 0) {
            remainingTime = cooldown;

            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = muzzle.position;
            projectile.transform.rotation = muzzle.rotation;

            IHomingRocket homingRocket = projectile.GetComponent<IHomingRocket>();
            if (homingRocket != null) {
                homingRocket.SetTarget(other.transform);
            }
        }
    }
}
