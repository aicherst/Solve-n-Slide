using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiocontroller : MonoBehaviour {

	public static AudioClip jetpackSound;
	public static AudioClip raiseHillSound;
	public static AudioClip lowerHillSound;
	public static AudioClip fuelTankPlacementSound;
	public static AudioClip fuelTankPickupSound;
	public static AudioClip loseHealthSound;
	public static AudioClip reachedGoalSound;
	public static AudioClip deathSound;
	//public static AudioClip switchPhasesSound; // ??????
	public static AudioSource windSound;

	public AudioClip jetpackSoundPrefab;
	public AudioClip raiseHillSoundPrefab;
	public AudioClip lowerHillSoundPrefab;
	public AudioClip fuelTankPlacementSoundPrefab;
	public AudioClip fuelTankPickupSoundPrefab;
	public AudioClip loseHealthSoundPrefab;
	public AudioClip reachedGoalSoundPrefab;
	public AudioClip deathSoundPrefab;
	//public AudioClip switchPhasesSoundPrefab; // ??????
	public AudioClip windSoundPrefab;

	// Use this for initialization
	void Start () {
		jetpackSound = jetpackSoundPrefab;
		raiseHillSound = raiseHillSoundPrefab;
		lowerHillSound = lowerHillSoundPrefab;
		fuelTankPlacementSound = fuelTankPlacementSoundPrefab;
		fuelTankPickupSound = fuelTankPickupSoundPrefab;
		loseHealthSound = loseHealthSoundPrefab;
		reachedGoalSound = reachedGoalSoundPrefab;
		deathSound = deathSoundPrefab;
		//switchPhasesSound = switchPhasesSoundPrefab;
		windSound = gameObject.GetComponent<AudioSource>();
		windSound.clip = windSoundPrefab;

		windSound.loop = true;
		Audiocontroller.windSound.volume = 0.1f;
	}

	public static void playJetpackSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(jetpackSound, v3);
	}

	public static void playRaiseHillSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(raiseHillSound, v3);
	}

	public static void playLowerHillSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(lowerHillSound, v3);
	}

	public static void playFuelTankPlacementSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(fuelTankPlacementSound, v3);
	}

	public static void playFuelTankPickupSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(fuelTankPickupSound, v3);
	}

	public static void playLoseHealthSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(loseHealthSound, v3);
	}

	public static void playReachedGoalSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(reachedGoalSound,v3);
	}

	public static void playDeathSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(deathSound, v3);
	}

	/*public static void playSwitchPhasesSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(switchPhasesSound, v3);
	}*/

	public static void playWindSound () {
		//AudioSource.PlayClipAtPoint(windSound, new Vector3(0f, 0f, 0f));
		windSound.Play();
	}
}
