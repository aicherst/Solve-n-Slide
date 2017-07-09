using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainUtil {
    public class TerrainTransform {
        private float realXToTerrainXMultiplier, realZToTerrainYMultiplier;

        private float terrainXToRealXMultiplier, terrainYToRealZMultiplier;
        private float heightToRealYMultiplier;

        private Vector3 realXYZOffset;

        private int heightmapWidth, heightmapHeight;

        public TerrainTransform(Terrain terrain) {
            Update(terrain);
        }

        public void Update(Terrain terrain) {
            TerrainData terrainData = terrain.terrainData;

            heightmapWidth = terrainData.heightmapWidth;
            heightmapHeight = terrainData.heightmapHeight;

            realXToTerrainXMultiplier = (heightmapWidth - 1) / terrainData.size.x;
            realZToTerrainYMultiplier = (heightmapHeight - 1) / terrainData.size.z;

            terrainXToRealXMultiplier = 1 / realXToTerrainXMultiplier;
            terrainYToRealZMultiplier = 1 / realZToTerrainYMultiplier;

            heightToRealYMultiplier = terrainData.size.y;

            realXYZOffset = terrain.transform.position;
        }

        public bool ContainsRealXZ(float x, float z) {
            int terrainX = RealXToTerrainX(x);

            if (terrainX < 0 || terrainX >= heightmapWidth)
                return false;

            int terrainY = RealZToTerrainY(z);

            if (terrainY < 0 || terrainY >= heightmapHeight)
                return false;

            return true;
        }

        public Vector3 Rasterize(Vector3 pos) {
            return new Vector3(TerrainXToRealX(RealXToTerrainX(pos.x)), pos.y, TerrainYToRealZ(RealZToTerrainY(pos.z)));
        }

        // Base functions: real to terrain

        public int RealXToTerrainX(float x) {
            return (int)((x - realXYZOffset.x) * realXToTerrainXMultiplier);
        }

        public int RealZToTerrainY(float z) {
            return (int)((z - realXYZOffset.z) * realZToTerrainYMultiplier);
        }

        // Base functions: terrain to real

        public float TerrainXToRealX(int x) {
            return TerrainXDistToRealXDist(x) + realXYZOffset.x;
        }

        public float TerrainXDistToRealXDist(int x) {
            return x * terrainXToRealXMultiplier;
        }

        public float TerrainYToRealZ(int y) {
            return y * terrainYToRealZMultiplier + realXYZOffset.z;
        }

        public float HeightToRealY(float height) {
            return height * heightToRealYMultiplier + realXYZOffset.y;
        }

        // compound functions

        public IntVector2 RealXZToTerrainXY(float x, float z) {
            return new IntVector2(RealXToTerrainX(x), RealZToTerrainY(z));
        }

        public Vector3 TerrainXZYToRealXYZ(int terrainX, int terrainY, float height) {
            return new Vector3(TerrainXToRealX(terrainX), HeightToRealY(height), TerrainYToRealZ(terrainY));
        }

        public Vector3 TerrainXZYToRealXYZ(IntVector2 terrainXY, float height) {
            return new Vector3(TerrainXToRealX(terrainXY.x), HeightToRealY(height), TerrainYToRealZ(terrainXY.y));
        }
    }
}