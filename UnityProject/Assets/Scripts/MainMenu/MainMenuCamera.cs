using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainMenuCamera : MonoBehaviour {
    private Camera _camera;

    public Vector2 sensitivity = new Vector2(180, 180);

    private float pitch = 0;
    private float yaw = 0;

    void Start() {
        _camera = GetComponent<Camera>();
        ActiveCamera.camera.SetData(_camera);
    }

    void Update() {
        yaw += Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;

        pitch += Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -50, 80);

        _camera.transform.localRotation = Quaternion.Euler(-pitch, yaw, 0);
    }
}