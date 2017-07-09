using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManipulationPhase {
    public class TerrainManipulationMarker : MonoBehaviour {
        public Color terrainIncrease = Color.red;
        public Color terrainDecrease = Color.blue;

        [HideInInspector]
        public HeightmapChangeData heightmapChangeData;

        private Color color {
            get {
                return heightmapChangeData.raise ? terrainIncrease : terrainDecrease;
            }
        }

        private void Start() {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color);
            gameObject.AddComponent<MouseOverHighlight>().hightlightStrengthEmission = 0.5f;
        }
    }
}
