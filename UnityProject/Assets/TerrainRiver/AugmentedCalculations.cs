using UnityEngine;

namespace RiverSimulation {
    public class AugementedCalculations : TerrainInterface {
        private float[,] heightmap;
        private Vector2[,] gradientmap;


        private int width, height;
        private int baseWidth, baseHeight;

        private bool gradientNormalized;

        private static readonly Neighbour[] _neighbours = new Neighbour[] {
            new Neighbour(IntVector2.right), new Neighbour(IntVector2.up), new Neighbour(IntVector2.left), new Neighbour(IntVector2.down),
            new Neighbour(IntVector2.one), new Neighbour(-IntVector2.one)
        };



        //public AugementedCalculations(float[] baseheightmap, int baseWidth, bool gradientNormalized = false) : this (Util.Convert(baseheightmap, baseWidth), gradientNormalized) {

        //}

        public AugementedCalculations(float[,] baseheightmap, bool gradientNormalized = false) {
            this.gradientNormalized = gradientNormalized;

            baseWidth = baseheightmap.GetLength(0);
            baseHeight = baseheightmap.GetLength(1);

            width = baseWidth + 2;
            height = baseHeight + 2;

            CreateHeightmapWithDuplicateBorder(baseheightmap);

            CreateGradientmap();
        }

        public Neighbour[] neighbours {
            get {
                return _neighbours;
            }
        }

        private void CreateHeightmapWithDuplicateBorder(float[,] baseheightmap) {
            heightmap = new float[width, height];

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    heightmap[x, y] = GetBaseHeight(baseheightmap, y, x);       // TODO remove inverse ?
                }
            }
        }

        private float GetBaseHeight(float[,] baseheightmap, int x, int y) {
            int baseX = x;
            int baseY = y;

            if (x > baseWidth) {
                baseX -= 2;
            } else if (x > 0) {
                baseX -= 1;
            }

            if (y > baseHeight) {
                baseY -= 2;
            } else if (y > 0) {
                baseY -= 1;
            }

            return baseheightmap[baseX, baseY];
        }

        private int ConvertBaseXYToBasePos(int x, int y) {
            return x + y * baseWidth;
        }

        private void CreateGradientmap() {
            gradientmap = new Vector2[width, height];

            for (int y = 1; y <= baseHeight; y++) {
                for (int x = 1; x <= baseWidth; x++) {
                    float slopeX, slopeY;

                    if (heightmap[x + 1, y] < heightmap[x - 1, y]) {
                        slopeX = Mathf.Max(0, heightmap[x, y] - heightmap[x + 1, y]);
                    } else {
                        slopeX = Mathf.Min(-0, heightmap[x - 1, y] - heightmap[x, y]);
                    }

                    if (heightmap[x, y + 1] < heightmap[x, y - 1]) {
                        slopeY = Mathf.Max(0, heightmap[x, y] - heightmap[x, y + 1]);
                    } else {
                        slopeY = Mathf.Min(-0, heightmap[x, y - 1] - heightmap[x, y]);
                    }

                    Vector2 gradient = new Vector2(slopeX, slopeY);

                    gradientmap[x, y] = gradientNormalized ? gradient.normalized : gradient;
                }
            }
        }

        // --- Retrieve Height ---

        public float GetHeightOfBasePos(IntVector2 basePos) {
            return heightmap[basePos.x + 1, basePos.y + 1];
        }

        public float GetHeightOfBasePos(Vector2 basePos) {
            return HeightOfPos(basePos + Vector2.one);
        }

        // Bi-linear Interpolation
        //private float HeightOfPos(Vector2 pos) {
        //    int intX = (int)pos.x;
        //    int intY = (int)pos.y;

        //    float dX = pos.x - intX;
        //    float dY = pos.y - intY;

        //    return heightmap[intX, intY] * (1 - dX) * (1 - dY) + heightmap[intX + 1, intY] * dX * (1 - dY) +
        //            heightmap[intX, intY + 1] * (1 - dX) * dY + heightmap[intX + 1, intY + 1] * dX * dY;
        //}

        private float triangleAreaInverse = 2f;
        private float triangleArea = 0.5f;

        // Barycentric Interpolation
        private float HeightOfPos(Vector2 pos) {
            int intX = (int)pos.x;
            int intY = (int)pos.y;

            float dX = pos.x - intX;
            float dY = pos.y - intY;

            float A1, A2, A3;

            float h3;

            if (dX > dY) {
                A1 = (1 - dX) * 0.5f;
                A2 = dY * 0.5f;
                h3 = heightmap[intX + 1, intY];
            } else {
                A1 = (1 - dY) * 0.5f;
                A2 = dX * 0.5f;
                h3 = heightmap[intX, intY + 1];
            }

            A3 = triangleArea - (A1 + A2);

            return (heightmap[intX, intY] * A1 + heightmap[intX + 1, intY + 1] * A2 + h3 * A3) * triangleAreaInverse;
        }

        //  --- Retrieve Gradient ---

        public Vector2 GradientOfBasePos(int baseX, int baseY) {
            return gradientmap[baseX + 1, baseY + 1];
        }

        public Vector2 GetGradientOfBasePos(Vector2 basePos) {
            return GradientOfPos(basePos + Vector2.one);
        }

        private Vector2 GradientOfPos(Vector2 pos) {            // Wrong interpolation
            int intX = (int)pos.x;
            int intY = (int)pos.y;

            float dX = pos.x - intX;
            float dY = pos.y - intY;

            return gradientmap[intX, intY] * (1 - dX) * (1 - dY) + gradientmap[intX + 1, intY] * dX * (1 - dY) +
                    gradientmap[intX, intY + 1] * (1 - dX) * dY + gradientmap[intX + 1, intY + 1] * dX * dY;
        }
    }
}