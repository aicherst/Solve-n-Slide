using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserturret : MonoBehaviour {

	public Transform head;
	public Transform target;
	private Quaternion baseHeadRotation;

	public GameObject missilePrefab;
	public Transform muzzel;

	public float cooldown = 2.0f;
	private float onCooldown;

	// Use this for initialization
	void Start () {
		baseHeadRotation = head.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 direction = target.position - head.position;
		direction.y = 0;
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		head.rotation = lookRotation*baseHeadRotation;

	if (onCooldown <= 0) {
			GameObject missileInst = Instantiate (missilePrefab);
			missileInst.transform.position = muzzel.position;
			missileInst.transform.rotation = muzzel.rotation;
            missileInst.GetComponent<SimpleHomingRocket>().target = target;

			onCooldown = cooldown;
		}

		onCooldown -= Time.deltaTime;
	}
}
