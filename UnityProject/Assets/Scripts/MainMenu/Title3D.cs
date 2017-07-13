using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title3D : MonoBehaviour {
	// Use this for initialization
	void Start () {
		foreach(Transform child in transform) {
            SpringJoint springJoint = child.gameObject.AddComponent<SpringJoint>();
            springJoint.damper = 0.9f;

            child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            child.gameObject.GetComponent<MeshRenderer>().material.color = RandomVirileColor();

            child.transform.position += Vector3.up * Random.Range(-0.2f, 0.2f);
        }
	}

    private T PullRandom<T>(List<T> list) {
        int randomIndex = Random.Range(0, list.Count);
        T result = list[randomIndex];
        list.RemoveAt(randomIndex);
        return result;
    }

    private Color RandomVirileColor() {
        List<int> colorParts = new List<int>(new int[] { 0, 1, 2 });
        int mainColorIndex = PullRandom(colorParts);
        int secondaryColorIndex = PullRandom(colorParts);

        float mainColorValue = Random.Range(0.8f, 1);
        float secondaryColorValue = Random.Range(0, mainColorValue);

        float[] iColor = new float[3];
        iColor[mainColorIndex] = mainColorValue;
        iColor[secondaryColorIndex] = secondaryColorValue;

        return new Color(iColor[0], iColor[1], iColor[2], 1);
    }
}
