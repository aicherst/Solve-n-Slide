using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    [SerializeField]
    private LockType _lockType;

    private void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public LockType lockType {
        get {
            return _lockType;
        }
    }

    // Use this for
    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Manipulation:
                gameObject.SetActive(true);
                break;
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<ActionCharacter>().AddKey(lockType);
            gameObject.SetActive(false);
        }
    }
}
