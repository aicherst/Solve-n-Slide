using System.Collections;
using System.Collections.Generic;
using ManipulationPhase;
using UnityEngine;

public abstract class ChargedBaseGUI<T> : MonoBehaviour where T : IChargeBasedManipulation {
    public Transform chargeHolder;
    public GameObject chargePrefab;

    private T chargeBasedManipulator;

    private ToggleGUI[] toggleCharges;

    private void Start() {
        Player.mainPlayer.AddListener(OnMainPlayerChange);
    }

    private void OnMainPlayerChange(ReadOnlyProperty<Player> changedProperty, Player newData, Player oldData) {
        chargeBasedManipulator = default(T);

        if (newData != null) {
            ResetCharges();

            chargeBasedManipulator = newData.GetCharacterComponent<T>();

            toggleCharges = new ToggleGUI[chargeBasedManipulator.maxCharges];
            for (int i = 0; i < chargeBasedManipulator.maxCharges; i++) {
                GameObject gCharge = Instantiate(chargePrefab);
                toggleCharges[i] = gCharge.GetComponent<ToggleGUI>();
                gCharge.transform.SetParent(chargeHolder);
            }
        }
    }

    private void ResetCharges() {
        foreach (Transform child in chargeHolder) {
            Destroy(child.gameObject);
        }

        toggleCharges = null;
    }

    // Update is called once per frame
    void Update() {
        if (chargeBasedManipulator == null)
            return;

        for (int i = 0; i < chargeBasedManipulator.charges; i++) {
            toggleCharges[i].toggle = true;
        }

        for (int i = chargeBasedManipulator.charges; i < chargeBasedManipulator.maxCharges; i++) {
            toggleCharges[i].toggle = false;
        }
    }
}
