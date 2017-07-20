using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManipulationPhase {
    public class FuelTankPlacer : MonoBehaviour, IChargeBasedManipulation {
        private static readonly float MAX_SCROLL = 2.5f;

        [SerializeField]
        private Camera mCamera;

        [SerializeField]
        public GameObject fuelTankPreview;
        [SerializeField]
        public GameObject fuelTankPrefab;

        [SerializeField]
        private int _maxFuelTanks = 3;

        private float scroll = 1;

        private int _fuelTanks;


        private Color fuelTankPreviewColor;
        private Color fuelTankPreviewColorInvalid;
        private MeshRenderer fuelTankPreviewMeshRenderer;

        private List<GameObject> placedFuelTanks = new List<GameObject>();

        public int maxCharges {
            get {
                return _maxFuelTanks;
            }
        }

        public int charges {
            get {
                return _fuelTanks;
            }
        }

        void Start() {
            _fuelTanks = _maxFuelTanks;

            if (mCamera == null) {
                Debug.LogWarning("No Terrain Manipulation Camera assigned. Using Camera.main");
                mCamera = Camera.main;
            }

            {
                fuelTankPreviewColor = fuelTankPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
                fuelTankPreviewMeshRenderer = fuelTankPreview.GetComponentInChildren<MeshRenderer>();
                fuelTankPreviewColorInvalid = fuelTankPreviewMeshRenderer.material.color;
                fuelTankPreviewColor.a = fuelTankPreviewColorInvalid.a;
            }

            GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
            ManipulationStateManager.instance.manipulationState.AddListener(OnManipulationStateChange);
        }

        public void OnManipulationStateChange(ReadOnlyProperty<ManipulationState> changedProperty, ManipulationState newData, ManipulationState oldData) {
            switch (newData) {
                case ManipulationState.FuelTankPlacement:
                    fuelTankPreview.SetActive(true);
                    break;
                default:
                    fuelTankPreview.SetActive(false);
                    break;
            }
        }

        public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
            switch (newData) {
                case GamePhase.Action:
                case GamePhase.Manipulation:
                    foreach(GameObject fuelTank in placedFuelTanks) {
                        fuelTank.SetActive(true);
                    }
                    break;
            }
        }

        private float distance {
            get {
                return Mathf.Pow(2, scroll) + 0.5f;
            }
        }

        void LateUpdate() {
            if (GameStateManager.instance.inputLock.data == InputLock.PauseMenu)
                return;

            if (ManipulationStateManager.instance.manipulationState.data != ManipulationState.FuelTankPlacement)
                return;

            Ray mouseRay = MouseInput.CameraRay(mCamera);

            scroll = Mathf.Clamp(scroll + Input.GetAxis("Mouse ScrollWheel"), 0, MAX_SCROLL);


            {
                RaycastHit hit;
                if (scroll == MAX_SCROLL && Physics.Raycast(mouseRay, out hit, float.PositiveInfinity, Layers.terrain)) {
                    fuelTankPreview.transform.position = hit.point + fuelTankPreview.GetComponentInChildren<MeshRenderer>().bounds.extents.y * Vector3.up;
                } else {
                    fuelTankPreview.transform.position = mouseRay.GetPoint(distance);
                }
            }

            fuelTankPreview.transform.LookAt(new Vector3(mCamera.transform.position.x, fuelTankPreview.transform.position.y, mCamera.transform.position.z));

            GameObject mouseOverFuelTank = null;

            {
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit) && hit.collider.CompareTag(Tags.fuelTank)) {
                    fuelTankPreview.SetActive(false);
                    mouseOverFuelTank = hit.collider.gameObject;
                } else {
                    fuelTankPreview.SetActive(true);
                }
            }

            if (Input.GetMouseButton(1) && mouseOverFuelTank != null) {
                Destroy(mouseOverFuelTank);
                _fuelTanks++;
            }


            if (_fuelTanks == 0) {
                fuelTankPreviewMeshRenderer.material.color = fuelTankPreviewColorInvalid;
            } else {
                fuelTankPreviewMeshRenderer.material.color = fuelTankPreviewColor;

                if (Input.GetMouseButtonDown(0) && mouseOverFuelTank == null) {
                    CreateFuelTank();
                }
            }
        }

        private void CreateFuelTank() {
            GameObject gFuelTank = Instantiate(fuelTankPrefab);
            gFuelTank.transform.SetParent(transform);
            gFuelTank.transform.position = fuelTankPreview.transform.position;
            gFuelTank.transform.rotation = fuelTankPreview.transform.rotation;

            gFuelTank.AddComponent<MouseOverHighlight>().hightlightStrengthColor = 0.4f;

            placedFuelTanks.Add(gFuelTank);

            _fuelTanks--;
        }

        private void OnDestroy() {
            GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
            ManipulationStateManager.instance.manipulationState.RemoveListener(OnManipulationStateChange);
        }
    }
}