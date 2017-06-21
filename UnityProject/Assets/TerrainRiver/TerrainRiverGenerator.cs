using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TerrainUtil;

namespace RiverSimulation {
    public struct MeshData {
        public Vector3[] verticies;
        public int[] triangles;
    }

    public class TerrainRiverChangeData {
        public List<MeshData> waterBodies;
        public Dictionary<IntVector2, float> terrainChangeData;
        public List<MeshData> rivers;

        public TerrainRiverChangeData() {
            waterBodies = new List<MeshData>();
            terrainChangeData = new Dictionary<IntVector2, float>();
            rivers = new List<MeshData>();
        }

        public void AddHeightChange(IntVector2 pos, float height) {
            terrainChangeData[pos] = height;
        }

        public void AddMaxHeightChange(IntVector2 pos, float newHeight) {
            float oldHeight;
            if (!terrainChangeData.TryGetValue(pos, out oldHeight) || newHeight > oldHeight) {
                terrainChangeData[pos] = newHeight;
            }
        }
    }

    public class TerrainRiverGenerator {
        private float[,] heightmap;

        private AugementedCalculations augementedCalculations;
        private TerrainTransform terrainTransform;

        public TerrainRiverGenerator(TerrainTransform terrainTransform, float[,] heightmap) {
            this.terrainTransform = terrainTransform;

            augementedCalculations = new AugementedCalculations(heightmap, true);
        }

        private float targetHeight;

        public void SetTargetHeight(float targetHeight) {
            this.targetHeight = targetHeight;
        }

        public TerrainRiverChangeData Generate(float realX, int realZ) {
            TerrainRiverChangeData calculationData = new TerrainRiverChangeData();

            TerrainRiverSimulation sinkFiller = new TerrainRiverSimulation(augementedCalculations);

            List<List<IntVector2>> paths = sinkFiller.Complete(terrainTransform.RealXZToTerrainXY(realX, realZ), targetHeight);
            List<IWaterBody> waterbodies = sinkFiller.GetWaterBodies();



            HashSet<IntVector2> allWaterPoints = new HashSet<IntVector2>();
            Dictionary<IntVector2, float> pointToWaterHeight = new Dictionary<IntVector2, float>();

            foreach (List<IntVector2> path in paths) {
                foreach (IntVector2 pos in path) {
                    allWaterPoints.Add(pos);
                }
            }


            foreach (List<IntVector2> path in paths) {
                IntVector2 nextPos = -IntVector2.one;

                for (int i = path.Count - 1; i >= 0; i--) {
                    IntVector2 pos = path[i];
                    float waterHeight = SurroundingMinHeight(pos, nextPos);
                    float originalHeight = augementedCalculations.GetHeightOfBasePos(pos);

                    pointToWaterHeight[pos] = waterHeight;

                    calculationData.AddHeightChange(pos, waterHeight - 0.02f);

                    foreach (Neighbour neighbour in augementedCalculations.neighbours) {
                        IntVector2 neighbourPos = pos + neighbour.direction;
                        if (allWaterPoints.Contains(neighbourPos))
                            continue;

                        float newNeighbourHeight = waterHeight + 0.01f;

                        if (newNeighbourHeight > augementedCalculations.GetHeightOfBasePos(neighbourPos))
                            continue;

                        calculationData.AddMaxHeightChange(neighbourPos, newNeighbourHeight);
                    }
                }
            }

            for (int i = 0; i < waterbodies.Count; i++) {
                float waterHeight = waterbodies[i].absoluteHeight;
                foreach (IntVector2 point in waterbodies[i].points) {
                    allWaterPoints.Add(point);
                    pointToWaterHeight[point] = waterHeight;
                }
            }

            calculationData.waterBodies.Add(CreateRiver(pointToWaterHeight));

            return calculationData;
        }

        private MeshData CreateRiver(Dictionary<IntVector2, float> posToHeight) {
            Dictionary<IntVector2, float> inflatedTerrainXYToHeight = InflatePoints(posToHeight);

            Func<IntVector2, Vector3> terrainXZYToRealXYZ = terrainXY => terrainTransform.TerrainXZYToRealXYZ(terrainXY, inflatedTerrainXYToHeight[terrainXY]);

            return CalculateMeshData(inflatedTerrainXYToHeight.Keys, terrainXZYToRealXYZ);
        }

        private float SurroundingMinHeight(IntVector2 pos, IntVector2 ignoredPos) {
            float minHeight = 1;
            foreach (Neighbour neighbour in augementedCalculations.neighbours) {
                IntVector2 neighbourPos = pos + neighbour.direction;
                if (neighbourPos == ignoredPos)
                    continue;
                minHeight = Mathf.Min(minHeight, augementedCalculations.GetHeightOfBasePos(neighbourPos));
            }
            return minHeight;
        }

        private HashSet<IntVector2> InflatePoints(IEnumerable<IntVector2> points) {
            HashSet<IntVector2> inflatedPoints = new HashSet<IntVector2>();

            foreach (IntVector2 pos in points) {
                inflatedPoints.Add(pos + IntVector2.right);
                inflatedPoints.Add(pos + IntVector2.up);
                inflatedPoints.Add(pos + IntVector2.left);
                inflatedPoints.Add(pos + IntVector2.down);
            }

            return inflatedPoints;
        }

        private Dictionary<IntVector2, float> InflatePoints(Dictionary<IntVector2, float> posToHeight) {
            Dictionary<IntVector2, float> inflatedPosToHeight = new Dictionary<IntVector2, float>();

            foreach (IntVector2 pos in posToHeight.Keys) {
                float height = posToHeight[pos];
                AddTo(inflatedPosToHeight, pos + IntVector2.right, height);
                AddTo(inflatedPosToHeight, pos + IntVector2.up, height);
                AddTo(inflatedPosToHeight, pos + IntVector2.left, height);
                AddTo(inflatedPosToHeight, pos + IntVector2.down, height);
            }


            foreach (IntVector2 pos in posToHeight.Keys) {
                inflatedPosToHeight[pos] = posToHeight[pos];
            }

            return inflatedPosToHeight;
        }

        private void AddTo(Dictionary<IntVector2, float> posToHeight, IntVector2 pos, float height) {
            float oldHeight;

            if (posToHeight.TryGetValue(pos, out oldHeight)) {
                height = (height + oldHeight) * 0.5f;
            }

            posToHeight[pos] = height;
        }

        private MeshData CalculateMeshData(IEnumerable<IntVector2> points, Func<IntVector2, Vector3> convertPos) {
            Dictionary<IntVector2, int> posToIndex = new Dictionary<IntVector2, int>();

            Vector3[] verticies = new Vector3[points.Count()];

            {
                int index = 0;
                foreach (IntVector2 pos in points) {
                    verticies[index] = convertPos(pos);/**/
                    posToIndex[pos] = index;
                    index++;
                }
            }

            MeshData mesh = new MeshData();
            mesh.verticies = verticies;
            mesh.triangles = CalculateTriangles(posToIndex);

            return mesh;
        }


        private int[] CalculateTriangles(Dictionary<IntVector2, int> posToIndex) {
            List<Triangle> triangleList = new List<Triangle>();

            foreach (IntVector2 pos in posToIndex.Keys) {
                int indexMM = posToIndex[pos];

                int indexUM, indexUR, indexMR;
                bool nUM, nUR, nMR;
                nUM = posToIndex.TryGetValue(pos + new IntVector2(0, 1), out indexUM);
                nUR = posToIndex.TryGetValue(pos + new IntVector2(1, 1), out indexUR);
                nMR = posToIndex.TryGetValue(pos + new IntVector2(1, 0), out indexMR);

                if (nUR) {
                    if (nUM) {              // ◤
                        triangleList.Add(new Triangle(indexMM, indexUM, indexUR));
                    }

                    if (nMR) {              // ◢
                        triangleList.Add(new Triangle(indexMM, indexUR, indexMR));
                    }
                } else {
                    if (nUM && nMR) {       // ◣
                        triangleList.Add(new Triangle(indexMM, indexUM, indexMR));
                    }
                }

                {                           // ◥
                    int indexML;
                    bool nML = posToIndex.TryGetValue(pos + new IntVector2(-1, 0), out indexML);

                    int indexDM;
                    bool nDM = posToIndex.TryGetValue(pos + new IntVector2(0, -1), out indexDM);
                    bool nDL = posToIndex.ContainsKey(pos + new IntVector2(-1, -1));

                    if (nML && nDM && !nDL) {
                        triangleList.Add(new Triangle(indexMM, indexDM, indexML));
                    }
                }
            }

            int[] triangles = new int[triangleList.Count * 3];
            for (int i = 0; i < triangleList.Count; i++) {
                triangles[i * 3 + 0] = triangleList[i].i1;
                triangles[i * 3 + 1] = triangleList[i].i2;
                triangles[i * 3 + 2] = triangleList[i].i3;

            }

            return triangles;
        }

        private struct Triangle {
            public int i1, i2, i3;

            public Triangle(int i1, int i2, int i3) {
                this.i1 = i1;
                this.i2 = i2;
                this.i3 = i3;
            }
        }

        public Vector3 ConvertPos(Vector3 pos) {
            return new Vector3(pos.x, pos.y * 10, pos.z);
        }
    }
}