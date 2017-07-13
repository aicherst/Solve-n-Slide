using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour {
    public Camera mCamera;

    public Vector2 sensitivity = new Vector2(180, 180);

    private float pitch = -80;
    private float yaw = 0;

    public Transform droneRoot;

    private void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Manipulation:
                mCamera.gameObject.SetActive(true);
                ActiveCamera.camera.SetData(mCamera);
                break;
            default:
                mCamera.gameObject.SetActive(false);
                break;
        }
    }

    void Update() {
        mCamera.transform.position = droneRoot.position;

        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Manipulation)
            return;


        yaw += Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;

        pitch += Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90, -30);

        mCamera.transform.localRotation = Quaternion.Euler(0, yaw, 0) * Quaternion.Euler(-pitch, 0, 0);
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }
}
