using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile3D : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach(Transform child in transform) {
            SpringJoint springJoint = child.gameObject.AddComponent<SpringJoint>();
            springJoint.damper = 0.9f;

            child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            child.gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

            child.transform.position += Vector3.up * Random.Range(-0.2f, 0.2f);
        }
	}
}
