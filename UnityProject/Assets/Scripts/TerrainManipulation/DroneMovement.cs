using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneMovement : MonoBehaviour {
    public Camera mCamera;

    public Vector3 speed = new Vector3(1, 0.5f, 1);

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Manipulation)
            return;
        

        float multiplier = Input.GetButton("DroneBoost") ? 2 : 1;

        float[] rawAxis = new float[] { Input.GetAxis("Horizontal"), Input.GetAxis("Drone Y"), Input.GetAxis("Vertical") };

        _rigidbody.velocity += Quaternion.Euler(0, mCamera.transform.eulerAngles.y, 0) * new Vector3(rawAxis[0] * speed.x, rawAxis[1] * speed.y, rawAxis[2] * speed.z) * multiplier;
    }
}
