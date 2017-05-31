using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turretrange : MonoBehaviour {

	//Turmparts
	public GameObject tt_;
	public GameObject parent0;
	public GameObject tt_1;
	public GameObject tt_2;

	public GameObject player = GameObject.Find("Player");

	Vector3 lookPos = new Vector3();

	//Variablen fuer den Cooldown. (Turm soll alle paar Sekunden feuern)
	public float sec;
	float timeStamp;
	float counterstart = 3.0f;

	public bool player_entering;

	Quaternion init_tt_ ;

	//Variablen fuer die Doppel-Projektile
	public GameObject Bullet_Emitter;
	public GameObject Bullet;
	public float Bullet_Forward_Force;


	void Start () {
		
		tt_ = GameObject.Find ("tt_");
		parent0 = GameObject.Find ("Parent0");
		tt_1 = GameObject.Find ("tt_1");

		player_entering = false;

		init_tt_ = tt_.transform.rotation;
	}

	void Update () {

		if (player_entering == true) {

			//Der Kopf des Geschuetzturmes verfolgt den Spieler und rotiert dabei nur um die y-Achse
			//Die Meshparts parent0, tt_1 und tt_2 sind Kinder von tt_ und bewegen sich korrekt mit (bis auf das Problem bei lookPos)
			tt_.transform.rotation = Quaternion.Slerp (transform.rotation, player.transform.rotation, Time.deltaTime * 2.0f);

			//Der Lauf des Geschuetzturmes soll zusaetzlich den Spieler verfolgen und dabei um das Mesh Parent0 um die xz-Ebene rotieren. Klappt noch nicht so ganz..
			lookPos = new Vector3 (player.transform.position.x, player.transform.position.z, 0);
			//Solange der Befehl hier auskommentiert ist, sieht es ok aus
			//	parent0.transform.LookAt (lookPos);
			
			//Ab der counterstart-en Sekunde wird alle sec Sekunden gefeuert
			if (Time.time > counterstart + sec) {
				//Animation fuer die Rueckstosskraft attached am Meshpart tt_2
				tt_2.GetComponent<Animation> ().Play ();


				//Projektile abfeuern
				//Sie werden in der falschen Orientierung abgefeuert. kA wieso
				//Diese Art von Projektil schiesst einfach nur gerade aus auf den Spieler zu ohne ihn zu verfolgen
				GameObject Temporary_Bullet_Handler;
				Temporary_Bullet_Handler = Instantiate (Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
				Temporary_Bullet_Handler.transform.Rotate (Vector3.left * 90);

				Rigidbody Temporary_Rigidbody;
				Temporary_Rigidbody = Temporary_Bullet_Handler.GetComponent<Rigidbody> ();
				Temporary_Rigidbody.AddForce (transform.forward * Bullet_Forward_Force);

				Destroy (Temporary_Bullet_Handler, 10.0f);

				counterstart = Time.time;
			}

		} 

		else {
			//Wenn der Spieler ausserhalb der Schussweite ist, dreht sich der Kopf des Geschuetzturmes wieder zur Anfangsposition
			tt_.transform.rotation = Quaternion.Slerp (transform.rotation, init_tt_, Time.deltaTime * 2.0f);
		}
	}

	//Ist der Spieler in Schussweite?
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			player_entering = true;
		}
	}

	//Ist der Spieler ausserhalb der Schussweite?
	void OnTriggerExit(Collider other){
		if(other.tag == "Player"){
			player_entering = false;
		}
	}
}
