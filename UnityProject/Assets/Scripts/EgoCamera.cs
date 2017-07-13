using UnityEngine;
using System.Collections;

public class EgoCamera : MonoBehaviour {
    public Camera _camera;

    public Transform eyes;

    public Vector2 sensitivity = new Vector2(180, 180);

    private float rotationY;

    void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
                rotationY = 0F;
                _camera.gameObject.SetActive(true);
                ActiveCamera.camera.SetData(_camera);
                break;
            case GamePhase.Manipulation:
                _camera.gameObject.SetActive(false);
                break;
        }
    }

    void Update() {
        GameStateManager gameStateManager = GameStateManager.instance;

        if (gameStateManager.inputLock.data == InputLock.PauseMenu)
            return;

        if (gameStateManager.gamePhase.data != GamePhase.Action && gameStateManager.gamePhase.data != GamePhase.Finished)
            return;

        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;

        rotationY += Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -50, 85);

        _camera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotationX, 0);

        _camera.transform.position = eyes.position; 
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }
}