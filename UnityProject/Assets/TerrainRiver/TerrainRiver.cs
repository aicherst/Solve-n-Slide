using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverSimulation {
    public class TerrainRiver : MonoBehaviour {
        public Terrain terrain;

        public GameObject waterBodyPrefab;

        [Range(0, 1)]
        public float targetHeight = 0.05f;

        private TerrainData terrainData;
        private TerrainRiverGenerator terrainRiverGenerator;

        private GameObject gWaterBodyParent;

        private int lastStartX, lastStartY;

        private List<TerrainChangeData> terrainChanges;

        private TerrainTransform terrainTransform;

        // Use this for initialization
        void Start() {
            terrainData = terrain.terrainData;

            float[,] heightmap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

            terrainTransform = new TerrainTransform(terrain);

            terrainRiverGenerator = new TerrainRiverGenerator(terrainTransform, heightmap);

            gWaterBodyParent = new GameObject();
            gWaterBodyParent.name = "Water Bodies";
        }

        void Update() {
            int startX = (int)transform.position.x;
            int startY = (int)transform.position.z;

            if (startX != lastStartX || startY != lastStartY) {
                terrainRiverGenerator.SetTargetHeight(targetHeight);

                CreateWaterBodies();
                lastStartY = startY;
                lastStartX = startX;
            }
        }

        private void Clear() {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in gWaterBodyParent.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));

            ResetTerrain();
        }

        private void ResetTerrain() {
            if (terrainChanges != null) {
                foreach (TerrainChangeData terrainChangeData in terrainChanges) {
                    terrainData.SetHeights(terrainChangeData.terrainX, terrainChangeData.terrainY, terrainChangeData.oldHeights);
                }
            }
        }

        void OnDestroy() {
            ResetTerrain();
        }

        private void CreateWaterBodies() {
            Clear();

            if (!terrainTransform.ContainsRealXZ(transform.position.x, transform.position.z)) {
                Debug.LogWarning("River start position out of bounds");
                return;
            }

            TerrainRiverChangeData terrainRiverChangeData = terrainRiverGenerator.Generate((int)transform.position.x, (int)transform.position.z);

            foreach (MeshData meshData in terrainRiverChangeData.waterBodies) {
                CreateWaterBody(meshData);
            }

            foreach (MeshData meshData in terrainRiverChangeData.rivers) {
                CreateWaterBody(meshData);
            }

            terrainChanges = terrainRiverChangeData.riverSink;

            foreach (TerrainChangeData terrainChangeData in terrainChanges) {
                terrainData.SetHeights(terrainChangeData.terrainX, terrainChangeData.terrainY, terrainChangeData.newHeights);
            }
        }

        private void CreateWaterBody(MeshData meshData) {
            GameObject gWaterBody = Instantiate(waterBodyPrefab);
            gWaterBody.transform.parent = gWaterBodyParent.transform;

            Mesh mesh = new Mesh();
            mesh.name = "Water Body";
            mesh.vertices = meshData.verticies;
            mesh.triangles = meshData.triangles;
            gWaterBody.GetComponent<MeshFilter>().mesh = mesh;
        }

        private void OnDrawGizmos() {
            if (terrain != null) {
                TerrainTransform terrainTransform = new TerrainTransform(terrain);

                if (terrainTransform.ContainsRealXZ(transform.position.x, transform.position.z)) {
                    Gizmos.color = Color.green;

                    int terrainX = terrainTransform.RealXToTerrainX(transform.position.x);
                    int terrainY = terrainTransform.RealZToTerrainY(transform.position.z);
                    float realY = terrain.terrainData.GetHeight(terrainX, terrainY) + terrain.transform.position.y;
                    float realX = terrainTransform.TerrainXToRealX(terrainX);
                    float realZ = terrainTransform.TerrainYToRealZ(terrainY);

                    Gizmos.DrawWireSphere(new Vector3(realX, realY, realZ), 0.5f);
                    Gizmos.DrawRay(new Vector3(realX, realY, realZ), Vector3.up * 2);
                } else {
                    Gizmos.color = Color.red;

                    Gizmos.DrawWireSphere(transform.position, 0.5f);
                    Gizmos.DrawWireSphere(transform.position, 0.25f);
                }
            }
        }
    }
}
