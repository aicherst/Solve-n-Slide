using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGUI : MonoBehaviour {
    [SerializeField]
    private Image background;

    public void SetColor(Color color) {
        background.color = color;
    }
}
