﻿using System.Collections.Generic;

namespace RiverSimulation {
    public class TerrainRiverSimulation {
        private static readonly int MAX_ITERATIONS = 50;

        private TerrainInterface terrainInterface;

        private Dictionary<IntVector2, WaterBody> posToWaterBody;

        private List<WaterBody> waterBodies;

        private WaterBody currentWaterBody;

        public TerrainRiverSimulation(TerrainInterface terrainInterface) {
            this.terrainInterface = terrainInterface;

            posToWaterBody = new Dictionary<IntVector2, WaterBody>();

            waterBodies = new List<WaterBody>();
        }

        public List<List<IntVector2>> Complete(IntVector2 startPos, float targetHeight) {
            List<List<IntVector2>> paths = new List<List<IntVector2>>();

            int counter = 0;

            while (counter < MAX_ITERATIONS) {
                List<IntVector2> path = Fall(startPos, targetHeight);
                paths.Add(path);

                IntVector2 endPoint = path[path.Count - 1];


                if (terrainInterface.GetHeightOfBasePos(endPoint) <= targetHeight) {
                    break;
                }
                WaterBody waterBody = Fill(endPoint);

                if (waterBody == null)
                    break;

                startPos = waterBody.drainPos;

                counter++;
            }

            RemoveContainedPaths(paths);

            return paths;
        }

        private void RemoveContainedPaths(List<List<IntVector2>> paths) {
            for (int i = 0; i < paths.Count; i++) {
                List<IntVector2> path = paths[i];

                int endPos = path.Count - 1;

                WaterBody startWaterBody;
                if (posToWaterBody.TryGetValue(path[0], out startWaterBody)) {
                    WaterBody endWaterBody;
                    if (posToWaterBody.TryGetValue(path[endPos], out endWaterBody)) {

                        if (endWaterBody == startWaterBody) {
                            paths.RemoveAt(i);
                            i--;
                            continue;
                        }

                    } else {
                        return;
                    }
                }

                while (endPos > 0 && posToWaterBody.ContainsKey(path[endPos])) {
                    endPos--;
                }

                path.RemoveRange(endPos + 2, path.Count - endPos - 2);
            }
        }

        private WaterBody Fill(IntVector2 startPos) {
            if (posToWaterBody.ContainsKey(startPos)) {
                return null;
            }

            currentWaterBody = new WaterBody();
            waterBodies.Add(currentWaterBody);

            currentWaterBody._absoluteHeight = terrainInterface.GetHeightOfBasePos(startPos);

            currentWaterBody.AddToDo(startPos, currentWaterBody._absoluteHeight);


            while (currentWaterBody.HasNext()) {
                IntVector2 currentPos = currentWaterBody.Next();

                {
                    WaterBody containingWaterBody;
                    if (posToWaterBody.TryGetValue(currentPos, out containingWaterBody)) {
                        if (containingWaterBody != currentWaterBody) {
                            Integrate(containingWaterBody);
                        }

                        continue;
                    }
                }


                float height = terrainInterface.GetHeightOfBasePos(currentPos);

                if (currentWaterBody._absoluteHeight > height) {
                    currentWaterBody.drainPos = GetKnownNeighbour(currentPos);
                    return currentWaterBody;
                }


                currentWaterBody._absoluteHeight = terrainInterface.GetHeightOfBasePos(currentPos); // Retrieved twice (=> change Heap)

                AddClosed(currentPos);

                foreach (Neighbour neighbour in terrainInterface.neighbours) {
                    IntVector2 neighbourPos = currentPos + neighbour.direction;

                    if (currentWaterBody.AlreadyKnown(neighbourPos)) {
                        continue;
                    }

                    currentWaterBody.AddToDo(neighbourPos, terrainInterface.GetHeightOfBasePos(neighbourPos));
                }
            }

            return null;
        }

        private IntVector2 GetKnownNeighbour(IntVector2 pos) {
            foreach (Neighbour neighbour in terrainInterface.neighbours) {
                IntVector2 neighbourPos = pos + neighbour.direction;

                if (posToWaterBody.ContainsKey(neighbourPos))
                    return neighbourPos;
            }

            return IntVector2.zero;
        }


        private List<IntVector2> Fall(IntVector2 startPos, float targetHeight) {

            List<IntVector2> path = new List<IntVector2>();

            IntVector2 nextPos = startPos;

            //Heap<IntVector2> openSet = new Heap<IntVector2>();
            //HashSet<IntVector2> closedSet = new HashSet<IntVector2>();
            //Dictionary<IntVector2, IntVector2> cameFrom = new Dictionary<IntVector2, IntVector2>();

            //openSet.Set(startPos, terrainInterface.GetHeightOfBasePos(startPos));

            while (true) {
                IntVector2 currentPos = nextPos;

                float height = terrainInterface.GetHeightOfBasePos(currentPos);

                path.Add(currentPos);

                if (height <= targetHeight) {
                    break;
                }

                float minSlope = 0;
                foreach (Neighbour neighbour in terrainInterface.neighbours) {
                    IntVector2 neighbourPos = neighbour.direction + currentPos;

                    if (posToWaterBody.ContainsKey(neighbourPos))
                        continue;       // Ignore the neighbor which is already evaluated.

                    float neighbourHeight = terrainInterface.GetHeightOfBasePos(neighbourPos);
                    float slope = (neighbourHeight - height) * neighbour.inverseDistance;

                    if(slope < minSlope) {
                        minSlope = slope;
                        nextPos = neighbourPos;
                    }
                }

                if (minSlope == 0)
                    break;
            }

            return path;
        }

        private List<IntVector2> ReconstructPath(Dictionary<IntVector2, IntVector2> cameFrom, IntVector2 currentPos) {
            List<IntVector2> path = new List<IntVector2>();

            do {
                path.Add(currentPos);
            } while (cameFrom.TryGetValue(currentPos, out currentPos));

            path.Reverse();

            return path;
        }

        private void AddClosed(IntVector2 pos) {
            currentWaterBody.closedSet.Add(pos);
            posToWaterBody.Add(pos, currentWaterBody);
        }

        public List<IWaterBody> GetWaterBodies() {
            List<IWaterBody> result = new List<IWaterBody>();
            foreach (WaterBody waterBody in waterBodies) {
                result.Add(waterBody);
            }
            return result;
        }

        private void Integrate(WaterBody waterBody) {
            if (currentWaterBody.closedSet.Count < waterBody.closedSet.Count) {
                WaterBody tmp = currentWaterBody;
                currentWaterBody = waterBody;
                waterBody = tmp;
            }

            Merge(currentWaterBody, waterBody);
        }

        private void Merge(WaterBody mainWaterBody, WaterBody addWaterBody) {
            foreach (IntVector2 pos in addWaterBody.closedSet) {
                mainWaterBody.closedSet.Add(pos);
                posToWaterBody[pos] = mainWaterBody;
            }

            foreach (IntVector2 pos in addWaterBody.openSet) {
                if (mainWaterBody.closedSet.Contains(pos))
                    continue;
                mainWaterBody.openSet.Set(pos, terrainInterface.GetHeightOfBasePos(pos));
            }


            mainWaterBody._absoluteHeight = Max(mainWaterBody._absoluteHeight, addWaterBody._absoluteHeight);
            waterBodies.Remove(addWaterBody);
        }

        private float Max(float f1, float f2) {
            return f1 > f2 ? f1 : f2;
        }

        private class WaterBody : IWaterBody {
            public HashSet<IntVector2> closedSet;
            public Heap<IntVector2> openSet;

            public IntVector2 drainPos;
            public float _absoluteHeight;

            public HashSet<IntVector2> points {
                get {
                    return closedSet;
                }
            }

            public float absoluteHeight {
                get {
                    return _absoluteHeight;
                }
            }

            public WaterBody() {
                closedSet = new HashSet<IntVector2>();
                openSet = new Heap<IntVector2>();
            }

            public bool AlreadyKnown(IntVector2 pos) {
                return closedSet.Contains(pos) || openSet.Contains(pos);
            }

            public bool HasNext() {
                return openSet.Count > 0;
            }

            public IntVector2 Next() {
                return openSet.RemoveFirst();
            }

            public void AddToDo(IntVector2 pos, float fCost) {
                openSet.Set(pos, fCost);
            }
        }
    }
    public interface IWaterBody {
        float absoluteHeight {
            get;
        }
        HashSet<IntVector2> points {
            get;
        }
    }
}