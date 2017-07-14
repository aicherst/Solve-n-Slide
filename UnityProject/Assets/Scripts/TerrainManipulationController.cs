using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManipulationController  {
    private static TerrainManipulationController _instance;

    public System.Action preChange;

    public static TerrainManipulationController instance {
        get {
            if(_instance == null) {
                _instance = new TerrainManipulationController();
            }
            return _instance;
        }
    }

    public void InformPreChange() {
        //preChange
    }

    public void InformPostChange() {

    }
}
