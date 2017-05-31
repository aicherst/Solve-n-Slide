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
    public Text levelFinishText;
    public Image levelFinishBackground;
    public Text deathText;
    public Image deathBackground;
    public Player player;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        healthText.enabled = false;
        healthBackground.enabled = false;
        fuelText.enabled = false;
        fuelBackground.enabled = false;
        chargesText.enabled = false;
        chargesBackground.enabled = false;
        levelFinishText.enabled = false;
        levelFinishBackground.enabled = false;
        deathText.enabled = false;
        deathBackground.enabled = false;

        if (player.getCurrentPhase() == Player.Phase.MANIPULATION_PHASE) {
            chargesText.enabled = true;
            chargesBackground.enabled = true;
            chargesText.text = "Test Charges: " + player.getManipulationCharacter().getCharges() + "/" + player.getManipulationCharacter().getMaxCharges();
        } else if (player.getCurrentPhase() == Player.Phase.ACTION_PHASE) {
            if (player.getActionCharacter().getLevelFinished()) {
                levelFinishBackground.enabled = true;
                levelFinishText.enabled = true;
            } else if (player.getActionCharacter().getDead()) {
                deathBackground.enabled = true;
                deathText.enabled = true;
            } else {
                healthText.enabled = true;
                healthBackground.enabled = true;
                healthText.text = "Test Health: " + makeThreeChars(player.getActionCharacter().getHealth()) + "/"
                    + makeThreeChars(player.getActionCharacter().getMaxHealth());

                fuelText.enabled = true;
                fuelBackground.enabled = true;
                fuelText.text = "Test Fuel: " + makeThreeChars(Mathf.RoundToInt(player.getActionCharacter().getFuel())) + "/"
                    + makeThreeChars(Mathf.RoundToInt(player.getActionCharacter().getMaxFuel()));
            }
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
