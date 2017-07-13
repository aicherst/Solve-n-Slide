using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Turret))]
public class DisplayRange : MonoBehaviour {
    public Projector rangeIndicator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!Application.isPlaying && rangeIndicator != null) {
            Turret turret = GetComponent<Turret>();
            rangeIndicator.orthographicSize = turret.range;
        }
	}
}
