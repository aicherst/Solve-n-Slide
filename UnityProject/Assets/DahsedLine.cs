using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DahsedLine : MonoBehaviour {
    public float speed = 1;
    private Material material;

	// Use this for initialization
	void Start () {
        material = GetComponent<LineRenderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        material.mainTextureOffset += new Vector2(-speed * Time.deltaTime, 0);
    }
}
