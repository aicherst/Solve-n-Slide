using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Key))]
public class KeySphere : MonoBehaviour {
    public MeshRenderer sphereRenderer;
	
	// Update is called once per frame
	void Update () {
        if (!Application.isPlaying) {
            LockType lockType = GetComponent<Key>().lockType;

            Color color = LockTypeToColor.Convert(lockType);
            color.a = sphereRenderer.material.color.a;
            sphereRenderer.material.color = color;
            sphereRenderer.material.SetColor("_RimColor", color);
        }
    }
}
