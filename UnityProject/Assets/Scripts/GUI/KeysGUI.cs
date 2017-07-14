using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysGUI : MonoBehaviour {
    public Transform keyHolder;
    public GameObject keyPrefab;

    private ActionCharacter actionCharacter;

    private void Start() {
        Player.mainPlayer.AddListener(OnMainPlayerChange);
    }

    private void OnMainPlayerChange(ReadOnlyProperty<Player> changedProperty, Player newData, Player oldData) {
        actionCharacter = null;

        if (newData != null) {
            ResetCharges();

            actionCharacter = newData.GetCharacterComponent<ActionCharacter>();
        }
    }

    private void ResetCharges() {
        foreach (Transform child in keyHolder) {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        if (actionCharacter == null)
            return;

        ResetCharges();

        foreach (LockType lockType in actionCharacter.keys) {
            Color color = LockTypeToColor.Convert(lockType);

            GameObject gKey = Instantiate(keyPrefab);
            gKey.GetComponent<KeyGUI>().SetColor(color);
            gKey.transform.SetParent(keyHolder);
        }
    }

    private void OnDestroy() {
        Player.mainPlayer.RemoveListener(OnMainPlayerChange);
    }
}
