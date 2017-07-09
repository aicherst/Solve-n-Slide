using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StayOnTerrain : MonoBehaviour {
    public Terrain terrain;

    public float  yOffset;

    public bool activateInEditMode = false;

    void Start() {
        if(Application.isPlaying && terrain == null) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position + Vector3.up * 1000, Vector3.down, out hit, float.PositiveInfinity, Layers.terrain)) {
                terrain = hit.collider.GetComponent<Terrain>();
                yOffset = transform.position.y - hit.point.y;
            }

            if(terrain == null) {
                Debug.LogError("No terrain found => disabled");
                enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        Terrain lTerrain = terrain;

        if(!Application.isPlaying) {
            if (!activateInEditMode)
                return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 1000, Vector3.down, out hit, float.PositiveInfinity, Layers.terrain)) {
                lTerrain = hit.collider.GetComponent<Terrain>();

                if (lTerrain == null)
                    return;
            }
        } 

        if(lTerrain != null) {
            transform.position = new Vector3(transform.position.x, lTerrain.SampleHeight(transform.position) + lTerrain.transform.position.y + yOffset, transform.position.z);
        }
    }
}
