using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && MouseInput.mouseOver.data == gameObject) {
            Application.Quit();
        }
    }
}
