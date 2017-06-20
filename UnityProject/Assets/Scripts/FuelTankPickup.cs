using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTankPickup : MonoBehaviour {

	public int fuelGain = 20;
	public static List<GameObject> fuelTanks;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "FuelTank") {
			gameObject.GetComponent<Jetpack>().fuel += fuelGain;

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

	public static void changeAllFuelTanksLayersForManipulationPhase () {
		foreach (GameObject nextFuelTank in fuelTanks) {
			nextFuelTank.layer = 10;
		}
	}

	public static void changeAllFuelTanksLayersForActionPhase () {
		foreach (GameObject nextFuelTank in fuelTanks) {
			nextFuelTank.layer = 2;
		}
	}

	public static void resetAllFuelTanks () {
		for (int i = fuelTanks.Count-1; i >= 0; i--) {
			Destroy(fuelTanks[i].gameObject);
			fuelTanks.RemoveAt(i);
		}
	}
}
