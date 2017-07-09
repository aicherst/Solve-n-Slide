using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainUtil;


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

        private float[,] originalHeightmap;

        private TerrainTransform terrainTransform;

        private GameObject[] destroyables;

        void Awake() {
            if (terrain == null)
                terrain = Terrain.activeTerrain;
        }

        // Use this for initialization
        void Start() {
            terrainData = terrain.terrainData;

            gWaterBodyParent = new GameObject();
            gWaterBodyParent.name = "Water Bodies";

            destroyables = GameObject.FindGameObjectsWithTag(Tags.destroyable);

            Simulate();
        }

        public void Simulate() {
            Clear();

            float[,] heightmap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

            terrainTransform = new TerrainTransform(terrain);

            terrainRiverGenerator = new TerrainRiverGenerator(terrainTransform, heightmap);

            terrainRiverGenerator.SetTargetHeight(targetHeight);

            CreateWaterBodies();
        }

        public void DestroyIntersectingDestroyables() {
            foreach (GameObject gObject in destroyables) {
                Vector3 position = gObject.GetComponent<IDestroyable>().GetPosition();

                MeshCollider[] meshColliders = gWaterBodyParent.GetComponentsInChildren<MeshCollider>();
                foreach (MeshCollider meshCollider in meshColliders) {
                    RaycastHit hit;
                    if (meshCollider.Raycast(new Ray(position + Vector3.up, Vector3.down), out hit, 10)) {
                        gObject.GetComponent<IDestroyable>().SetDestroyed(true);
                    }
                }
            }
        }

        public void ResetIntersectingDestroyables() {
            foreach (GameObject gObject in destroyables) {
                gObject.GetComponent<IDestroyable>().SetDestroyed(false);
            }
        }

        public void Clear() {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in gWaterBodyParent.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));

            ResetIntersectingDestroyables();

            ResetTerrain();
        }

        private void ResetTerrain() {
            if (originalHeightmap == null)
                return;

            terrainData.SetHeights(0, 0, originalHeightmap);
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


            originalHeightmap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);


            Dictionary<IntVector2, float> terrainChanges = terrainRiverChangeData.terrainChangeData;

            foreach (IntVector2 pos in terrainChanges.Keys) {
                terrainData.SetHeights(pos.x, pos.y, new float[,]{{terrainChanges[pos]} });
            }
        }

        private void CreateWaterBody(MeshData meshData) {
            GameObject gWaterBody = Instantiate(waterBodyPrefab);
            gWaterBody.transform.parent = gWaterBodyParent.transform;

            Mesh mesh = new Mesh();
            mesh.name = "Water Body";
            mesh.vertices = meshData.verticies;
            mesh.triangles = meshData.triangles;
            mesh.RecalculateNormals();
            gWaterBody.GetComponent<MeshFilter>().mesh = mesh;

            MeshCollider meshCollider = gWaterBody.GetComponent<MeshCollider>();
            if(meshCollider != null) {
                meshCollider.sharedMesh = mesh;
            }
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
