using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysAndDoors : MonoBehaviour {

	public static GameObject[] keyObjects;
	public static GameObject[] doorObjects;

	public static int keys = 0;

	private void Start () {
		keyObjects = GameObject.FindGameObjectsWithTag("Key");
		doorObjects = GameObject.FindGameObjectsWithTag("Door");
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Key") {
			keys++;
			other.gameObject.SetActive(false);
		}
		else if (other.tag == "Door" && keys >= 1) {
			keys--;
			other.transform.parent.gameObject.SetActive(false);
		}
	}

	public static void activateAllKeysAndDoors() {
		Debug.Log(keyObjects.Length);
		for (int i = 0; i < keyObjects.Length; i++) {
			keyObjects[i].SetActive(true);
		}
			
		for (int i = 0; i < doorObjects.Length; i++) {
			doorObjects[i].transform.parent.gameObject.SetActive(true);
		}
	}
}
