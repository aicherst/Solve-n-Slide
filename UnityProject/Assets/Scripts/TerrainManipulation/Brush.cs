using UnityEngine;

namespace ManipulationPhase {
    [System.Serializable]
    public class Brush {
        public Texture2D brush;
        public float scale = 1;
        public float heightDifference = 1;
        public int smooth = 1;

        private float[,] lastScaledBrushHeightmap;

        public float[,] heights {
            get {
                RecalcScaledBrushHeightmap();

                return lastScaledBrushHeightmap;
            }
        }

        private void RecalcScaledBrushHeightmap() {
            lastScaledBrushHeightmap = new float[width, height];

            float invScale = 1 / scale;

            Color[] colors = brush.GetPixels();
            for (int y = 0; y < lastScaledBrushHeightmap.GetLength(1); y++) {
                for (int x = 0; x < lastScaledBrushHeightmap.GetLength(0); x++) {
                    lastScaledBrushHeightmap[x, y] = colors[(int)(x * invScale) + (int)(y * invScale) * brush.width].grayscale;
                }
            }

            for (int i = 0; i < smooth; i++) {
                Smooth(lastScaledBrushHeightmap);
            }
        }

        public int width {
            get {
                return (int)(brush.width * scale);
            }
        }

        public int height {
            get {
                return (int)(brush.height * scale);
            }
        }


        private void Smooth(float[,] array) {
            //Kernel kernel = new Kernel(new float[,] { { 0.07f, 0.07f, 0 }, { 0.07f, 0.58f, 0.07f }, { 0, 0.07f, 0.07f } });
            Kernel kernel = new Kernel(new float[,] { { 0.1f, 0.1f, 0 }, { 0.1f, 0.4f, 0.1f }, { 0, 0.1f, 0.1f } });
            //Kernel kernel = new Kernel(new float[,] { { 0, 0.07f, 0.07f }, { 0.07f, 0.58f, 0.07f }, { 0.07f, 0.07f, 0 } });

            for (int y = 1; y < array.GetLength(1) - 1; y++) {
                for (int x = 1; x < array.GetLength(0) - 1; x++) {
                    array[x, y] = kernel.Apply(array, x, y);
                }
            }
        }

        private struct Kernel {
            private float[,] matrix;

            public Kernel(float[,] matrix) {
                this.matrix = matrix;
            }

            public float Apply(float[,] array, int xCenter, int yCenter) {
                int xPos = xCenter - matrix.GetLength(0) / 2;
                int yPos = yCenter - matrix.GetLength(1) / 2;

                float result = 0;
                for (int y = 0; y < matrix.GetLength(1); y++) {
                    for (int x = 0; x < matrix.GetLength(0); x++) {
                        result += matrix[x, y] * array[xPos + x, yPos + y];
                    }
                }
                return result;
            }
        }
    }
}
