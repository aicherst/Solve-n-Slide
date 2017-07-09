using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeFollower : MonoBehaviour {
    public Transform cubeTransform;
    public float distance;
	
	// Update is called once per frame
	void Update () {
        if (cubeTransform != null) {
            transform.position = cubeTransform.position + cubeTransform.forward * -distance;
            transform.rotation = cubeTransform.rotation;
        }
    }
}
