using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverHighlight : MonoBehaviour {
    [Range(0, 1)]
    public float hightlightStrengthColor;
    [Range(0, 1)]
    public float hightlightStrengthEmission;

    private static readonly float FADE_TIME = 0.1f;

    private Color defaultColor;
    private Color defaultEmission;

    private Material material;

    private bool mouseOver;
    private float time;

    // Use this for initialization
    void Start() {
        material = GetComponentInChildren<MeshRenderer>().material;

        defaultColor = material.color;
        defaultEmission = material.GetColor("_EmissionColor");

        MouseInput.mouseOver.AddListener(OnMouseInputOver);
    }

    private void OnMouseInputOver(ReadOnlyProperty<GameObject> changedProperty, GameObject newData, GameObject oldData) {
        if (newData == gameObject) {
            mouseOver = true;
        } else {
            mouseOver = false;
        }
    }

    private void Update() {
        if (mouseOver) {
            time += Time.deltaTime;
        } else {
            time -= Time.deltaTime;
        }

        time = Mathf.Clamp(time, 0, FADE_TIME);

        float percentage = time / FADE_TIME;
        material.color = defaultColor + Color.white * Mathf.Lerp(0, hightlightStrengthColor, percentage);
        material.SetColor("_EmissionColor", defaultEmission + Color.white * Mathf.Lerp(0, hightlightStrengthEmission, percentage));
    }

    private void OnDestroy() {
        MouseInput.mouseOver.RemoveListener(OnMouseInputOver);
    }

    private void OnDisable() {
        ResetColors();
    }

    private void ResetColors() {
        material.color = defaultColor;
        material.SetColor("_EmissionColor", defaultEmission);
    }
}
