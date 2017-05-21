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
    public Character character;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        healthText.enabled = character.getCurrentPhase() == Character.Phase.ACTION_PHASE;
        healthBackground.enabled = character.getCurrentPhase() == Character.Phase.ACTION_PHASE;
        healthText.text = "Test Health: " + makeThreeChars(character.getHealth()) + "/" + makeThreeChars(character.getMaxHealth());

        fuelText.enabled = character.getCurrentPhase() == Character.Phase.ACTION_PHASE;
        fuelBackground.enabled = character.getCurrentPhase() == Character.Phase.ACTION_PHASE;
        fuelText.text = "Test Fuel: " + makeThreeChars(Mathf.RoundToInt(character.getFuel())) + "/" + makeThreeChars(Mathf.RoundToInt(character.getMaxFuel()));

        chargesText.enabled = character.getCurrentPhase() == Character.Phase.MANIPULATION_PHASE;
        chargesBackground.enabled = character.getCurrentPhase() == Character.Phase.MANIPULATION_PHASE;
        chargesText.text = "Test Charges: " + character.getCharges() + "/" + character.getMaxCharges();
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
