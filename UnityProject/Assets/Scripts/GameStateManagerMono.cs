using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManagerMono : MonoBehaviour {
    private void Update() {
        if (GameStateManager.instance.inputLock.data == InputLock.PauseMenu)
            return;

        if (Input.GetButtonDown("PhaseSwitch")) {
            switch (GameStateManager.instance.gamePhase.data) {
                case GamePhase.Manipulation:
                    GameStateManager.instance.gamePhase.SetData(GamePhase.Action);
                    break;
                case GamePhase.Finished:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                    break;
                case GamePhase.Dead:
                    GameStateManager.instance.gamePhase.SetData(GamePhase.Manipulation);
                    break;
                case GamePhase.Action:
                    GameStateManager.instance.gamePhase.SetData(GamePhase.Manipulation);
                    break;
            }
        }
    }

}

public class GameStateManager {
    private static GameStateManager _instance;

    private Property<GamePhase> _gamePhase = new Property<GamePhase>();
    private Property<InputLock> _inputLock = new Property<InputLock>();

    public static GameStateManager instance {
        get {
            if (_instance == null) {
                _instance = new GameStateManager();
            }
            return _instance;
        }
    }

    public Property<GamePhase> gamePhase {
        get {
            return _gamePhase;
        }
    }

    public Property<InputLock> inputLock {
        get {
            return _inputLock;
        }
    }
}

public enum GamePhase {
    Manipulation, Action, Finished, Dead
}

public enum InputLock {
    Game, PauseMenu
}
