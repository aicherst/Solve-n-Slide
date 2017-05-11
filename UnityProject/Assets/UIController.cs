using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text healthText;
    public Text fuelText;
    public Text chargesText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void setHealth(int health, int maxHealth) {
        string healthString;
        if (health < 10)
            healthString = "00" + health;
        else if (health < 100)
            healthString = "0" + health;
        else
            healthString = health.ToString();
        healthText.text = "Test Health: " + healthString + "/" + maxHealth;
    }

    void setFuel(int fuel, int maxFuel) {
        string fuelString;
        if (fuel < 10)
            fuelString = "00" + fuel;
        else if (fuel < 100)
            fuelString = "0" + fuel;
        else
            fuelString = fuel.ToString();
        healthText.text = "Test Fuel: " + fuelString + "/" + maxFuel;
    }

    void setCharges(int charges, int maxCharges) {
        healthText.text = "Test Fuel: " + charges + "/" + maxCharges;
    }
}
