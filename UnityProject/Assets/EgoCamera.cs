using UnityEngine;
using System.Collections;

public class EgoCamera : MonoBehaviour {
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    float rotationY = 0F;
    public Camera egoCamera;

    void Update() {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            egoCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
            transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }

    void Start() {
    }
}