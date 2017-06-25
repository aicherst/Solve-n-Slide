using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHomingRocket : MonoBehaviour, IHomingRocket {
    public float maxSpeed = 1;
    public AnimationCurve accelerationCurve;
    public float rotationSpeed = 120;              // degree per sec

    public float maxLifeLifeTime = 6f;

    public float maxDamage;
    public AnimationCurve damageFallOffCurve;

    public GameObject explosionPrefab;

    public Transform target;

    public float lockRotationTime = 1;

    private float lifeTime;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;

        if(lifeTime > maxLifeLifeTime) {
            Explode();
            return;
        }

        Vector3 targetDirection = (target.position - transform.position).normalized;

        if(lifeTime > lockRotationTime) {
            //create the rotation we need to be in to look at the target
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }


        Vector3 veclocity = transform.forward * accelerationCurve.Evaluate(lifeTime) * maxSpeed;

        transform.position += veclocity * Time.deltaTime;
    }

    private float CalculateDamage() {
        return damageFallOffCurve.Evaluate(Vector3.Distance(target.position, transform.position)) * maxDamage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == Tags.terrain) {
            Explode();
        }

        if (other.tag == Tags.player) {
            Explode();
        }
    }

    private void Explode() {
        GameObject gExplosion = Instantiate(explosionPrefab);
        gExplosion.transform.position = transform.position;
        Destroy(gameObject);

        Destroy(gExplosion, 3);

        ICharacter character = target.GetComponent<ICharacter>();

        float damage = CalculateDamage();

        if (character != null || damage <= 0) {
            character.damage(damage);
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }
}
