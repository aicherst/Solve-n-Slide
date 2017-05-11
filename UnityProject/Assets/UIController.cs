using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text healthText;
    public Text fuelText;
    public Text chargesText;
    public Character character;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        healthText.text = "Test Health: " + makeThreeChars(character.getHealth()) + "/" + makeThreeChars(character.getMaxHealth());
        fuelText.text = "Test Fuel: " + makeThreeChars(Mathf.RoundToInt(character.getFuel())) + "/" + makeThreeChars(Mathf.RoundToInt(character.getMaxFuel()));
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
