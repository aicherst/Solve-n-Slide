using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManipulationPhase {
    public class FuelTankPlacer : MonoBehaviour, IChargeBasedManipulation {
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

            Ray mouseRay = mCamera.ScreenPointToRay(Input.mousePosition);

            scroll = Mathf.Clamp(scroll + Input.GetAxis("Mouse ScrollWheel"), 0, 2.5f);
            fuelTankPreview.transform.position = mouseRay.GetPoint(distance);

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

            if(Input.GetMouseButton(1) && mouseOverFuelTank != null) {
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

            gFuelTank.AddComponent<MouseOverHighlight>().hightlightStrengthColor = 0.4f;

            _fuelTanks--;
        }
    }
}