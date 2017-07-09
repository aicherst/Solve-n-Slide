using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers {
    public static readonly int terrain = 1 << LayerMask.NameToLayer("Terrain");
    public static readonly int terrainMarker = 1 << LayerMask.NameToLayer("TerrainMarker");
    public static readonly int fuelTank = 1 << LayerMask.NameToLayer("FuelTank");
}
