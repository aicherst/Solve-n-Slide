using UnityEngine;

namespace RiverSimulation {
    public interface TerrainInterface {
        Vector2 GetGradientOfBasePos(Vector2 basePos);
        float GetHeightOfBasePos(Vector2 basePos);
        float GetHeightOfBasePos(IntVector2 basePos);

        Neighbour[] neighbours {
            get;
        }
    }

    public struct Neighbour {
        public IntVector2 direction;
        public float inverseDistance;

        public Neighbour(IntVector2 direction) {
            this.direction = direction;
            inverseDistance = 1 / direction.Magnitude();
        }
    }
}