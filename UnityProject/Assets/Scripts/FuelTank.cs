using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour {
    public int fuel = 20;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.player)) {
            other.GetComponent<Jetpack>().fuel += fuel;
			AudioManager.playFuelTankPickupSound(transform.position);
            gameObject.SetActive(false);
        }
    }
}
