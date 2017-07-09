using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    [SerializeField]
	private AudioClip jetpackSound;
    [SerializeField]
    private AudioClip raiseHillSound;
    [SerializeField]
    private AudioClip lowerHillSound;
    [SerializeField]
    private AudioClip fuelTankPlacementSound;
    [SerializeField]
    private AudioClip fuelTankPickupSound;
    [SerializeField]
    private AudioClip loseHealthSound;
    [SerializeField]
    private AudioClip reachedGoalSound;
    [SerializeField]
    private AudioClip deathSound;
    //[SerializeField]
    //public AudioClip switchPhasesSound; // ??????
    [SerializeField]
    private AudioClip windSound;


    private AudioSource windAudioSource;

    private static AudioManager instance;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        windAudioSource = gameObject.GetComponent<AudioSource>();
        windAudioSource.clip = windSound;
	}

	public static void playJetpackSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.jetpackSound, v3);
	}

	public static void playRaiseHillSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.raiseHillSound, v3);
	}

	public static void playLowerHillSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.lowerHillSound, v3);
	}

	public static void playFuelTankPlacementSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.fuelTankPlacementSound, v3);
	}

	public static void playFuelTankPickupSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.fuelTankPickupSound, v3);
	}

	public static void playLoseHealthSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.loseHealthSound, v3);
	}

	public static void playReachedGoalSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.reachedGoalSound,v3);
	}

	public static void playDeathSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.deathSound, v3);
	}

    /*public static void playSwitchPhasesSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.switchPhasesSound, v3);
	}*/

    public static void SetRelativeWindVolume (float volume) {
        instance.windAudioSource.volume = volume;
	}
}
