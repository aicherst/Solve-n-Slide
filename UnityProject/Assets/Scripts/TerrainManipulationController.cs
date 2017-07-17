using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainManipulationController  {
    private static TerrainManipulationController _instance;

    public Action preChange;
    public Action postChange;

    public static TerrainManipulationController instance {
        get {
            if(_instance == null) {
                _instance = new TerrainManipulationController();
            }
            return _instance;
        }
    }

    public void InformPreChange() {
        if (preChange == null)
            return;

        preChange.Invoke();
    }

    public void InformPostChange() {
        if (postChange == null)
            return;

        postChange.Invoke();
    }
}
