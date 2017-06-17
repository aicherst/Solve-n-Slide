using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWindSoundBasedOnSpeed : MonoBehaviour {

	private int speedStatus = 0;

	// Update is called once per frame
	void Update () {
		/*if (gameObject.GetComponent<CharacterMovement>().velocity.magnitude <= 3 && speedStatus != 0) {
			speedStatus = 0;
			Audiocontroller.windSound.Stop();
		}
		else if (3 < gameObject.GetComponent<CharacterMovement>().velocity.magnitude && gameObject.GetComponent<CharacterMovement>().velocity.magnitude <= 10 && speedStatus != 1) {
			speedStatus = 1;
			Audiocontroller.windSound.volume = 0.3f;
			Audiocontroller.windSound.Play();
		}
		else if (10 < gameObject.GetComponent<CharacterMovement>().velocity.magnitude && gameObject.GetComponent<CharacterMovement>().velocity.magnitude <= 20 && speedStatus != 2) {
			speedStatus = 2;
			Audiocontroller.windSound.volume = 0.6f;
			Audiocontroller.windSound.Play();
		}
		else if (20 < gameObject.GetComponent<CharacterMovement>().velocity.magnitude && speedStatus != 3) {
			speedStatus = 3;
			Audiocontroller.windSound.volume = 1.0f;
			Audiocontroller.windSound.Play();
		}*/
	}
}
