using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turretrange2 : MonoBehaviour {

	public GameObject tt2_;
	public GameObject parent1;
	public GameObject tt2_1;
	public GameObject tt2_2;

	public GameObject player = GameObject.Find("Player");

	Vector3 lookPos = new Vector3();

	float sec;
	float timeStamp;
	float counterstart = 3.0f;

	bool player_entering;

	Quaternion init_tt2_ ;


	void Start () {
		tt2_ = GameObject.Find ("tt2_");
		parent1 = GameObject.Find ("Parent1");
		tt2_1 = GameObject.Find ("tt2_1");

		player_entering = false;

		init_tt2_ = tt2_.transform.rotation;
	}

	void Update () {
		if (player_entering == true) {

			//Problem mit diesem Mesh, da Unity es immer in der falschen Orientierung importiert. Dadurch funktionieren die Parents nicht

			tt2_.transform.rotation = Quaternion.Slerp (transform.rotation, player.transform.rotation, Time.deltaTime * 2.0f);


			lookPos = new Vector3 (player.transform.position.x, player.transform.position.z, 0);
			//	parent0.transform.LookAt (lookPos);


			if (Time.time > counterstart + sec) {
				//Animation ist fertig, aber nicht attached
				tt2_2.GetComponent<Animation> ().Play ();

				//Bspw. Code fuer Rocket, der Spieler verfolgen soll einfuegen

				counterstart = Time.time;
			}

		} 

		else {
			tt2_.transform.rotation = Quaternion.Slerp (transform.rotation, init_tt2_, Time.deltaTime * 2.0f);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			player_entering = true;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Player"){
			player_entering = false;
		}
	}

}
