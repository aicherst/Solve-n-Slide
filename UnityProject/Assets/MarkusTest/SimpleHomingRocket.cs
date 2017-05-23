using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHomingRocket : MonoBehaviour {
    public float maxSpeed = 1;
    public AnimationCurve accelerationCurve;
    public float rotationSpeed = 10;              // degree per sec

    public float maxLifeLifeTime = 6f;

    public float maxDamage;
    public AnimationCurve damageFallOffCurve;

    public GameObject explosionPrefab;

    public Transform target;

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


        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


        Vector3 veclocity = targetDirection  * accelerationCurve.Evaluate(lifeTime) * maxSpeed;

        transform.position += veclocity * Time.deltaTime;
    }

    private float damage {
        get {
            return damageFallOffCurve.Evaluate(Vector3.Distance(target.position, transform.position));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == Tags.terrain) {
            Explode();
        }
    }

    private void Explode() {
        GameObject gExplosion = Instantiate(explosionPrefab);
        gExplosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
