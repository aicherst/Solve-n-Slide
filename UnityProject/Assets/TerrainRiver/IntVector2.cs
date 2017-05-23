using UnityEngine;

namespace RiverSimulation {
    public struct IntVector2 {
        public int x, y;

        public static readonly IntVector2 zero = new IntVector2(0, 0);

        public static readonly IntVector2 right = new IntVector2(1, 0);
        public static readonly IntVector2 left = new IntVector2(-1, 0);
        public static readonly IntVector2 up = new IntVector2(0, 1);
        public static readonly IntVector2 down = new IntVector2(0, -1);

        public static readonly IntVector2 one = new IntVector2(1, 1);


        public IntVector2(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public float Magnitude() {
            return Mathf.Sqrt(x * x + y * y);
        }

        public override int GetHashCode() {
            return x + y * 17;
        }

        public override bool Equals(object obj) {
            if (!(obj is IntVector2))
                return false;

            return this == (IntVector2)obj;
        }

        public static IntVector2 operator -(IntVector2 v) {
            return new IntVector2(-v.x, -v.y);
        }

        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2) {
            return new IntVector2(v1.x - v2.x, v1.x - v2.y);
        }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2) {
            return new IntVector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator *(IntVector2 v1, float s) {
            return new Vector2(v1.x + s, v1.y + s);
        }

        public static IntVector2 operator *(IntVector2 v1, int s) {
            return new IntVector2(v1.x + s, v1.y + s);
        }

        public static bool operator ==(IntVector2 v1, IntVector2 v2) {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(IntVector2 v1, IntVector2 v2) {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public static implicit operator Vector2(IntVector2 v) {
            return new Vector2(v.x, v.y);
        }

        public override string ToString() {
            return "(" + x + ", " + y + ")";
        }
    }
}
