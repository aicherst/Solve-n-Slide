using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text healthText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void setHealth(int health) {
        string healthString;
        if (health < 10)
            healthString = "00" + health;
        else if (health < 100)
            healthString = "0" + health;
        else
            healthString = health.ToString();
        healthText.text = "Test Health: " + healthString + "/100";
    }
}
