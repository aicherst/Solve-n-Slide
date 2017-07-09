using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BackgroundCamera : MonoBehaviour {
    private Camera activeCamera;
    private Camera _camera;

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
        ActiveCamera.camera.AddListener(OnActiveCameraChange);
    }

    // Use this for
    public void OnActiveCameraChange(ReadOnlyProperty<Camera> changedProperty, Camera newData, Camera oldData) {
        activeCamera = newData;

        if(activeCamera != null) {
            _camera.fieldOfView = activeCamera.fieldOfView;
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        if(activeCamera != null) {
            transform.position = activeCamera.transform.position;
            transform.rotation = activeCamera.transform.rotation;
        }
	}
}
