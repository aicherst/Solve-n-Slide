using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainUtil;

namespace ManipulationPhase {
    public class TerrainManipulator : MonoBehaviour, IChargeBasedManipulation {
        #region private inspector variables

        [SerializeField]
        private int _maxCharges = 5;

        [SerializeField]
        private Camera mCamera;

        [SerializeField]
        private Brush brush;

        [SerializeField]
        private Projector projector;

        [SerializeField]
        private GameObject markerPrefab;
        #endregion inspector variables

        #region private variables
        private Dictionary<HeightmapChangeData, GameObject> heightmapChangeToMarker;

        private List<GameObject> disabledTerrainMarkers;

        private int _charges;

        private TerrainManipulation terrainManipulation;

        private List<TerrainDataWithOriginalHeights> terrainsDataWithOriginalHeights;
        #endregion

        #region public members
        public int maxCharges {
            get {
                return _maxCharges;
            }
        }

        public int charges {
            get {
                return _charges;
            }
        }
        #endregion

        private void Start() {
            heightmapChangeToMarker = new Dictionary<HeightmapChangeData, GameObject>();
            disabledTerrainMarkers = new List<GameObject>();

            terrainManipulation = new TerrainManipulation();

            _charges = _maxCharges;

            if (mCamera == null) {
                Debug.LogWarning("No Terrain Manipulation Camera assigned. Using Camera.main");
                mCamera = Camera.main;
            }
            CreateFailSafeOriginalTerrainHeights();

            ManipulationStateManager.instance.manipulationState.AddListener(OnManipulationStateChange);
        }

        private void CreateFailSafeOriginalTerrainHeights() {
            terrainsDataWithOriginalHeights = new List<TerrainDataWithOriginalHeights>();

            foreach (Terrain terrain in FindObjectsOfType<Terrain>()) {
                TerrainData terrainData = terrain.terrainData;
                float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
                terrainsDataWithOriginalHeights.Add(new TerrainDataWithOriginalHeights(terrainData, heights));
            }
        }

        private void ResetToFailSafeOriginalTerrainHeights() {
            TerrainManipulationController.instance.InformPreChange();

            foreach (TerrainDataWithOriginalHeights terrainDataWithOriginalHeights in terrainsDataWithOriginalHeights) {
                TerrainData terrainData = terrainDataWithOriginalHeights.terrainData;
                float[,] heights = terrainDataWithOriginalHeights.heights;
                terrainData.SetHeights(0, 0, heights);
            }

            TerrainManipulationController.instance.InformPostChange();
        }

        private struct TerrainDataWithOriginalHeights {
            public TerrainData terrainData;
            public float[,] heights;

            public TerrainDataWithOriginalHeights(TerrainData terrainData, float[,] heights) {
                this.terrainData = terrainData;
                this.heights = heights;
            }
        }

        public void OnManipulationStateChange(ReadOnlyProperty<ManipulationState> changedProperty, ManipulationState newData, ManipulationState oldData) {
            switch (newData) {
                case ManipulationState.TerrainChange:
                    EnableTerrainHelpers();
                    break;
                default:
                    DisableTerrainHelpers();
                    break;
            }
        }

        private void EnableTerrainHelpers() {
            foreach (GameObject disabledTerrainMarker in disabledTerrainMarkers) {
                disabledTerrainMarker.SetActive(true);
            }

            disabledTerrainMarkers.Clear();

            projector.enabled = true;
        }

        private void DisableTerrainHelpers() {
            foreach (GameObject gMarker in heightmapChangeToMarker.Values) {
                if (gMarker.activeSelf) {
                    disabledTerrainMarkers.Add(gMarker);
                    gMarker.SetActive(false);
                }
            }

            projector.enabled = false;
        }

        // Update is called once per frame
        void Update() {
            if (GameStateManager.instance.inputLock.data == InputLock.PauseMenu)
                return;

            if (ManipulationStateManager.instance.manipulationState.data != ManipulationState.TerrainChange)
                return;

            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) {     // Undo
            //    HeightmapChangeData heightmapChangeData = terrainManipulation.Undo();

            //    // Disable Marker
            //    GameObject gMarker;
            //    if (heightmapChangeData != null && heightmapChangeToMarker.TryGetValue(heightmapChangeData, out gMarker)) {
            //        gMarker.SetActive(!gMarker.activeSelf);
            //    }
            //}
            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Y)) {     // Redo
            //    HeightmapChangeData heightmapChangeData = terrainManipulation.Redo();

            //    // Show Marker
            //    GameObject gMarker;
            //    if (heightmapChangeData != null && heightmapChangeToMarker.TryGetValue(heightmapChangeData, out gMarker)) {
            //        gMarker.SetActive(!gMarker.activeSelf);
            //    }
            //}

            if (Input.GetButtonDown("TerrainReset")) {     // Reset Terrain Changes
                ResetManipulation();
            }

            TerrainManipulationUpdate();
        }

        private bool IsUnmodifiable(Terrain terrain, Vector3 point) {
            return TerrainHelpers.GetMainTextureName(terrain, point).Contains("Unmodifiable");
        }

        private void TerrainManipulationUpdate() {
            Vector3 hitPoint = Vector3.zero;
            Terrain terrain = null;

            {
                RaycastHit hit;
                Ray ray = MouseInput.CameraRay(mCamera);
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, Layers.terrain)) {
                    hitPoint = hit.point;
                    terrain = hit.collider.GetComponent<Terrain>();
                    projector.enabled = true;
                } else {
                    projector.enabled = false;
                    return;
                }
            }

            TerrainData terrainData = terrain.terrainData;

            TerrainTransform terrainTransform = new TerrainTransform(terrain);

            hitPoint = terrainTransform.Rasterize(hitPoint);

            UpdateProjector(terrainTransform, hitPoint);

            HeightmapChangeData markerHeightmapChangeData = TryGetMarkerHeightmapChange();

            bool raise = Input.GetMouseButtonDown(0);
            bool lower = Input.GetMouseButtonDown(1);

            if (raise || lower) {                               // Terrain manipulation
                if (markerHeightmapChangeData != null && raise != markerHeightmapChangeData.raise) {    // Revert the mouse over terrain manipulation
                    TerrainManipulationController.instance.InformPreChange();

                    _charges++;

                    terrainManipulation.Revert(markerHeightmapChangeData);

                    heightmapChangeToMarker[markerHeightmapChangeData].SetActive(false);

                    AudioManager.PlayTerrainManipulationSound(mCamera.transform.position, raise);

                    TerrainManipulationController.instance.InformPostChange();
                } else if (_charges > 0 && !IsUnmodifiable(terrain, hitPoint)) {                        // Raise / lower terrain and create marker
                    TerrainManipulationController.instance.InformPreChange();

                    _charges--;

                    HeightmapChangeData heightmapChangeData = terrainManipulation.Manipulate(terrainData, hitPoint - terrain.transform.position, brush, raise);
                    CreateTerrainMarker(heightmapChangeData, terrainTransform, terrain, hitPoint, raise);

                    AudioManager.PlayTerrainManipulationSound(mCamera.transform.position, raise);

                    TerrainManipulationController.instance.InformPostChange();
                }
            }
        }

        private HeightmapChangeData TryGetMarkerHeightmapChange() {
            RaycastHit hit;
            Ray ray = MouseInput.CameraRay(mCamera);
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, Layers.water.Inverse()) && hit.collider.CompareTag(Tags.manipulationMarker)) {        // If mouse over manipulation marker
                return hit.collider.GetComponent<TerrainManipulationMarker>().heightmapChangeData;
            }
            return null;
        }

        private void CreateTerrainMarker(HeightmapChangeData heightmapChangeData, TerrainTransform terrainTransform, Terrain terrain, Vector3 point, bool raise) {
            float height = terrain.SampleHeight(point);

            GameObject gMarker = Instantiate(markerPrefab);
            gMarker.transform.position = new Vector3(point.x, height, point.z);
            gMarker.transform.localScale = Vector3.one * terrainTransform.TerrainXDistToRealXDist(2) * 2;

            gMarker.transform.SetParent(gameObject.transform);

            gMarker.GetComponent<TerrainManipulationMarker>().heightmapChangeData = heightmapChangeData;

            heightmapChangeToMarker[heightmapChangeData] = gMarker;
        }

        private void UpdateProjector(TerrainTransform terrainTransform, Vector3 point) {
            projector.transform.position = point + Vector3.up * 100;
            projector.material.SetTexture("_ShadowTex", brush.brush);
            projector.orthographicSize = terrainTransform.TerrainXDistToRealXDist(brush.width);
        }

        private void ResetManipulation() {
            TerrainManipulationController.instance.InformPreChange();

            terrainManipulation.Reset();
            _charges = _maxCharges;

            foreach (GameObject gMarker in heightmapChangeToMarker.Values) {
                Destroy(gMarker);
            }

            heightmapChangeToMarker.Clear();

            TerrainManipulationController.instance.InformPostChange();
        }

        private void OnDestroy() {
            ResetToFailSafeOriginalTerrainHeights();

            ManipulationStateManager.instance.manipulationState.RemoveListener(OnManipulationStateChange);
        }
    }


    public class TerrainManipulation {
        private List<HeightmapChange> doneHeightmapChanges;
        private List<HeightmapChange> undoneHeightmapChanges;

        public TerrainManipulation() {
            doneHeightmapChanges = new List<HeightmapChange>();
            undoneHeightmapChanges = new List<HeightmapChange>();
        }

        public void Reset() {
            doneHeightmapChanges.Reverse();

            foreach (HeightmapChange heightmapChange in doneHeightmapChanges) {
                heightmapChange.Undo();
            }
        }

        public HeightmapChangeData Undo() {
            if (doneHeightmapChanges.Count == 0)
                return null;

            HeightmapChange heightmapChange = doneHeightmapChanges[doneHeightmapChanges.Count - 1];

            heightmapChange.Undo();

            doneHeightmapChanges.RemoveAt(doneHeightmapChanges.Count - 1);
            undoneHeightmapChanges.Add(heightmapChange);

            return heightmapChange.heightmapChangeData;
        }

        public HeightmapChangeData Redo() {
            if (undoneHeightmapChanges.Count == 0)
                return null;

            HeightmapChange heightmapChange = undoneHeightmapChanges[undoneHeightmapChanges.Count - 1];

            heightmapChange.Redo();

            undoneHeightmapChanges.RemoveAt(undoneHeightmapChanges.Count - 1);
            doneHeightmapChanges.Add(heightmapChange);

            return heightmapChange.heightmapChangeData;
        }

        public void Revert(HeightmapChangeData heightmapChangeData) {
            HeightmapChange heightmapChange = new HeightmapChange(heightmapChangeData, !heightmapChangeData.raise);

            heightmapChange.Redo();

            doneHeightmapChanges.Add(heightmapChange);
        }


        private IntVector2 PointToGridPos(TerrainData terrainData, Vector3 point) {
            int x = (int)((terrainData.heightmapWidth - 1) / terrainData.size.x * point.x);
            int y = (int)((terrainData.heightmapHeight - 1) / terrainData.size.z * point.z);

            return new IntVector2(x, y);
        }

        public HeightmapChangeData Manipulate(TerrainData terrainData, Vector3 point, Brush brush, bool raise) {
            IntVector2 center = PointToGridPos(terrainData, point);
            HeightmapChangeData heightmapChangeData = CreateHeightmapChangeData(terrainData, center, brush, raise);

            HeightmapChange heightmapChange = new HeightmapChange(heightmapChangeData, raise);

            heightmapChange.Redo();
            doneHeightmapChanges.Add(heightmapChange);

            undoneHeightmapChanges.Clear();

            return heightmapChangeData;
        }

        private HeightmapChangeData CreateHeightmapChangeData(TerrainData terrainData, IntVector2 center, Brush brush, bool raise) {
            int posX = center.x - brush.width / 2;
            int posY = center.y - brush.height / 2;



            float[,] heights = brush.heights;

            float heightMultiplier = brush.heightDifference / terrainData.size.y;
            for (int y = 0; y < heights.GetLength(1); y++) {
                for (int x = 0; x < heights.GetLength(0); x++) {
                    heights[x, y] = heights[x, y] * heightMultiplier;
                }
            }

            return new HeightmapChangeData(terrainData, new IntVector2(posX, posY), heights, raise);
        }

        private class HeightmapChange {
            private HeightmapChangeData _heightmapChangeData;

            private bool add;

            public HeightmapChange(HeightmapChangeData heightmapChangeData, bool add) {
                _heightmapChangeData = heightmapChangeData;
                this.add = add;
            }


            public void Redo() {
                ApplyHeights(add);
            }

            public void Undo() {
                ApplyHeights(!add);
            }

            private void ApplyHeights(bool add) {
                TerrainData terrainData = _heightmapChangeData.terrainData;

                float[,] heights = _heightmapChangeData.heights;

                float[,] terrainHeights = terrainData.GetHeights(_heightmapChangeData.pos.x, _heightmapChangeData.pos.y, heights.GetLength(0), heights.GetLength(1));

                if (add) {
                    for (int y = 0; y < heights.GetLength(1); y++) {
                        for (int x = 0; x < heights.GetLength(0); x++) {
                            terrainHeights[x, y] += heights[x, y];
                        }
                    }
                } else {
                    for (int y = 0; y < heights.GetLength(1); y++) {
                        for (int x = 0; x < heights.GetLength(0); x++) {
                            terrainHeights[x, y] -= heights[x, y];
                        }
                    }
                }

                terrainData.SetHeights(_heightmapChangeData.pos.x, _heightmapChangeData.pos.y, terrainHeights);
            }

            public HeightmapChangeData heightmapChangeData {
                get {
                    return _heightmapChangeData;
                }
            }
        }
    }

    public class HeightmapChangeData {
        public TerrainData terrainData;
        public IntVector2 pos;
        public float[,] heights;

        public bool raise;

        public HeightmapChangeData(TerrainData terrainData, IntVector2 pos, float[,] heights, bool raise) {
            this.terrainData = terrainData;
            this.pos = pos;
            this.heights = heights;
            this.raise = raise;
        }
    }
}
