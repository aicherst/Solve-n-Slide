using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers {
    public static readonly AugmentedLayer terrain = new AugmentedLayer(LayerMask.NameToLayer("Terrain"));
    public static readonly AugmentedLayer terrainMarker = new AugmentedLayer(LayerMask.NameToLayer("TerrainMarker"));
    public static readonly AugmentedLayer fuelTank = new AugmentedLayer(LayerMask.NameToLayer("FuelTank"));
    public static readonly AugmentedLayer water = new AugmentedLayer(LayerMask.NameToLayer("Water"));
}

public struct AugmentedLayer {
    public int layer;

    public AugmentedLayer(int layer) {
        this.layer = layer;
    }

    public int Inverse() {
        return ~(1 << layer);
    }

    public static implicit operator int(AugmentedLayer l) {
        return 1 << l.layer;
    }
}