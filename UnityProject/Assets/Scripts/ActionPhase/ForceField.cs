using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Lock))]
public class ForceField : MonoBehaviour {
	// Use this for initialization
	void Start () {

        //color.a = sphereRenderer.material.color.a;
        //sphereRenderer.material.color = color;
        //sphereRenderer.material.SetColor("_RimColor", color);
    }

    private void Update() {
        if(!Application.isPlaying) {
            LockType lockType = GetComponent<Lock>().lockType;

            Color color = LockTypeToColor.Convert(lockType);
            color = ClampColor(color);


            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers) {
                meshRenderer.material.color = color;
            }
        }
    }

    private Color ClampColor(Color color) {
        float maxColor = Mathf.Max(color.r, color.g, color.b);
        float cMult = Mathf.Min(maxColor, 0.37f) / maxColor;
        return color * cMult;
    }
}
