using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSound : MonoBehaviour {
    [SerializeField]
    private float speedToVolume = 0.33f;

    private CharacterMovement characterMovement;

    private void Start() {
        characterMovement = gameObject.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update () {
        AudioManager.SetRelativeWindVolume(Mathf.Clamp01(characterMovement.velocity.magnitude * speedToVolume));
	}
}
