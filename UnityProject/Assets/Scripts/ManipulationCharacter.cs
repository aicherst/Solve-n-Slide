using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationCharacter : MonoBehaviour {
    public int charges = 5, maxCharges = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getCharges() { return charges; }
    public int getMaxCharges() { return maxCharges; }
}
