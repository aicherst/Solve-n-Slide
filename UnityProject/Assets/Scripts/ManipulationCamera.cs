using UnityEngine;
using System.Collections;

public class ManipulationCamera : MonoBehaviour {
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	float rotationY = 0F;
	public Camera egoCamera;

	void Update () {
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp(rotationY, -90, 90);

		egoCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		transform.localEulerAngles = new Vector3(0, rotationX, 0);

		transform.Translate(Vector3.right * Input.GetAxis("Horizontal"));
		transform.Translate(Vector3.forward * Input.GetAxis("Vertical"));
	}

	void Start () {
	}
}