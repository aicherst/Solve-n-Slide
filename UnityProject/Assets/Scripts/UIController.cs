using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text healthText;
    public Image healthBackground;
    public Text fuelText;
    public Image fuelBackground;
    public Text chargesText;
    public Image chargesBackground;
    //public ActionCharacter character;
    public Player player;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (player.getCurrentPhase() == Player.Phase.MANIPULATION_PHASE) {
            healthText.enabled = false;
            healthBackground.enabled = false;
            fuelText.enabled = false;
            fuelBackground.enabled = false;

            chargesText.enabled = true;
            chargesBackground.enabled = true;
            chargesText.text = "Test Charges: " + player.getManipulationCharacter().getCharges() + "/" + player.getManipulationCharacter().getMaxCharges();
        } else if (player.getCurrentPhase() == Player.Phase.ACTION_PHASE) {
            healthText.enabled = true;
            healthBackground.enabled = true;
            healthText.text = "Test Health: " + makeThreeChars(player.getActionCharacter().getHealth()) + "/" 
                + makeThreeChars(player.getActionCharacter().getMaxHealth());

            fuelText.enabled = true;
            fuelBackground.enabled = true;
            fuelText.text = "Test Fuel: " + makeThreeChars(Mathf.RoundToInt(player.getActionCharacter().getFuel())) + "/" 
                + makeThreeChars(Mathf.RoundToInt(player.getActionCharacter().getMaxFuel()));

            chargesText.enabled = false;
            chargesBackground.enabled = false;
        }
    }

    private string makeThreeChars(int value) {
        string valueString;
        if (value < 10)
            valueString = "00" + value;
        else if (value < 100)
            valueString = "0" + value;
        else
            valueString = value.ToString();
        return valueString;
    }
}
