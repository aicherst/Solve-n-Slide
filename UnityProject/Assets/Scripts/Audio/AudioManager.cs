using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
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

        windAudioSource.Play();

        AudioListener.volume = 0.2f;
    }

	public static void PlayTerrainManipulationSound (Vector3 v3, bool raise) {
        if(raise) {
            AudioSource.PlayClipAtPoint(instance.raiseHillSound, v3);
        } else {
            AudioSource.PlayClipAtPoint(instance.lowerHillSound, v3);
        }
    }

	public static void playFuelTankPlacementSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.fuelTankPlacementSound, v3);
	}

	public static void playFuelTankPickupSound (Vector3 v3) {
		AudioSource.PlayClipAtPoint(instance.fuelTankPickupSound, v3);
	}

	public static void PlayLoseHealthSound (Vector3 v3, float volume = 1) {
		AudioSource.PlayClipAtPoint(instance.loseHealthSound, v3, volume);
	}

	public static void PlayReachedGoalSound (Vector3 v3) {
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
