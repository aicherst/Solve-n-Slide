using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacterAnimation : MonoBehaviour {
    public Animator animator;
    private CharacterMovement characterMovement;

	// Use this for initialization
	void Start () {
        characterMovement = GetComponent<CharacterMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("grounded", characterMovement.grounded);
        animator.SetFloat("speed", Mathf.Clamp01(characterMovement.velocity.magnitude * 0.05f) );
    }
}
