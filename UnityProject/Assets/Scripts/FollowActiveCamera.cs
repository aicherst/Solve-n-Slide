using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActiveCamera : MonoBehaviour {
	// Update is called once per frame
	void LateUpdate () {
        Camera camera = ActiveCamera.camera.data;
		if(camera != null) {
            transform.position = camera.transform.position;
        }
	}
}
