using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputMono : MonoBehaviour {
    public Texture2D cursorIcon;

    // Use this for initialization
    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;

        if(cursorIcon != null) {
            Cursor.SetCursor(cursorIcon, new Vector2(cursorIcon.width, cursorIcon.height) * 0.5f, CursorMode.Auto);
        }
    }

    private void Start() {
        GameStateManager.instance.inputLock.AddListener(OnInputLockChange);
    }

    // Use this for
    public void OnInputLockChange(ReadOnlyProperty<InputLock> changedProperty, InputLock newData, InputLock oldData) {
        switch (newData) {
            case InputLock.PauseMenu:
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Ray ray = MouseInput.CameraRay(ActiveCamera.camera.data);
        if (Physics.Raycast(ray, out hit)) {
            MouseInput.mouseOver.SetData(hit.collider.gameObject);
        } else {
            MouseInput.mouseOver.SetData(null);
        }
    }
}

public class MouseInput {
    private static Property<GameObject> _mouseOver = new Property<GameObject>();

    public static Property<GameObject> mouseOver {
        get {
            return _mouseOver;
        }
    }

    public static Ray CameraRay(Camera camera) {
        if(Cursor.lockState == CursorLockMode.Locked) {
            return camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        } else {
            return camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
