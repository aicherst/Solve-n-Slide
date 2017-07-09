using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelGUI : MonoBehaviour {
    public Image fuelBarLeft, fuelBarRight;

    public Text fuelText;

    private static readonly float BAR_WIDTH = 209f;

    private Jetpack jetpack;

    private void Start() {
        Player.mainPlayer.AddListener(OnMainPlayerChange);
    }

    private void OnMainPlayerChange(ReadOnlyProperty<Player> changedProperty, Player newData, Player oldData) {
        jetpack = null;

        if (newData != null) {
            jetpack = newData.GetCharacterComponent<Jetpack>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (jetpack == null)
            return;

        float fuelPercentage = jetpack.fuelPercentage;
        fuelBarLeft.transform.localPosition = new Vector3(BAR_WIDTH * (1f - fuelPercentage), 0);
        fuelBarRight.transform.localPosition = new Vector3(-BAR_WIDTH * (1f - fuelPercentage), 0);

        fuelText.text = ((int) jetpack.fuel).ToString();

        //fuelBarLeft.color = new Color(1 - fuelPercentage, fuelPercentage, 0, 0.5f);
        //fuelBarRight.color = new Color(1 - fuelPercentage, fuelPercentage, 0, 0.5f);
    }
}
