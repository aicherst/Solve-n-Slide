using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {
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

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            ActionCharacter actionCharacter = other.GetComponent<ActionCharacter>();
            if (actionCharacter.HasKey(_lockType)) {
                actionCharacter.KeyUsed(_lockType);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }
}
