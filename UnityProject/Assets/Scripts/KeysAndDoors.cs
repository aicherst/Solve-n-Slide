using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysAndDoors : MonoBehaviour {

	public static int keys = 0;
	
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Key") {
			keys++;
			Destroy(other.gameObject);
		}
		else if (other.tag == "Door" && keys >= 1) {
			keys--;
			Destroy(other.transform.parent.gameObject);
		}
	}
}
