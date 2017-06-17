using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTankPickup : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.tag == "FuelTank") {
			gameObject.GetComponent<Jetpack>().fuel += 20;

			if (gameObject.GetComponent<Jetpack>().fuel > gameObject.GetComponent<Jetpack>().maxFuel) {
				gameObject.GetComponent<Jetpack>().fuel = gameObject.GetComponent<Jetpack>().maxFuel;
			}

			Destroy(other.gameObject);
		}
	}
}
