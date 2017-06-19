using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTankPickup : MonoBehaviour {

	public static List<GameObject> fuelTanks;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "FuelTank") {
			gameObject.GetComponent<Jetpack>().fuel += 20;

			if (gameObject.GetComponent<Jetpack>().fuel > gameObject.GetComponent<Jetpack>().maxFuel) {
				gameObject.GetComponent<Jetpack>().fuel = gameObject.GetComponent<Jetpack>().maxFuel;
			}

			other.gameObject.SetActive(false);
		}
	}

	public static void activateAllFuelTanks () {
		foreach (GameObject nextFuelTank in fuelTanks) {
			nextFuelTank.SetActive(true);
		}
	}

	public static void resetAllFuelTanks () {
		for (int i = fuelTanks.Count-1; i >= 0; i--) {
			Destroy(fuelTanks[i].gameObject);
			fuelTanks.RemoveAt(i);
		}
	}
}
