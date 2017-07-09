using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainUtil;

namespace ManipulationPhase {
    public class ManipulationStateManagerMono : MonoBehaviour {
        public bool terrainManipulation = true;
        public bool fuelTankPlacement = true;

        void Start() {
            GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
        }

        public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
            switch(newData) {
                case GamePhase.Manipulation:
                    if (!terrainManipulation) {
                        if (!fuelTankPlacement) {
                            ManipulationStateManager.instance.manipulationState.SetData(ManipulationState.Other);
                        } else {
                            ManipulationStateManager.instance.manipulationState.SetData(ManipulationState.FuelTankPlacement);
                        }
                    } else {
                        ManipulationStateManager.instance.manipulationState.SetData(ManipulationState.TerrainChange);
                    }
                    break;
                default:
                    ManipulationStateManager.instance.manipulationState.SetData(ManipulationState.Other);
                    break;
            }
        }


        private void Update() {
            GameStateManager gameStateManager = GameStateManager.instance;

            if (gameStateManager.inputLock.data == InputLock.PauseMenu)
                return;

            if (gameStateManager.gamePhase.data != GamePhase.Manipulation)
                return;

            if (Input.GetButtonDown("TerrainChange") && terrainManipulation) {
                ManipulationStateManager.instance.manipulationState.SetData(ManipulationState.TerrainChange);
            } else if(Input.GetButtonDown("FuelTankPlacement") && fuelTankPlacement) {
                ManipulationStateManager.instance.manipulationState.SetData(ManipulationState.FuelTankPlacement);
            }
        }
    }

    public enum ManipulationState {
        TerrainChange, FuelTankPlacement, Other
    }

    public class ManipulationStateManager {
        private static ManipulationStateManager _instance;

        private Property<ManipulationState> _manipulationState = new Property<ManipulationState>();

        public static ManipulationStateManager instance {
            get {
                if (_instance == null) {
                    _instance = new ManipulationStateManager();
                }
                return _instance;
            }
        }

        public Property<ManipulationState> manipulationState {
            get {
                return _manipulationState;
            }
        }
    }
}
