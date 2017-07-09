using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGUI : MonoBehaviour {
    [SerializeField]
    private bool _toogle = true;

    public GameObject on, off;

    public bool toggle {
        get {
            return _toogle;
        }
        set {
            if(value != _toogle) {
                _toogle = value;
                UpdateGameObjects();
            }
        }
    }

	// Use this for initialization
	void Awake () {
        UpdateGameObjects();
    }

    void UpdateGameObjects() {
        on.SetActive(_toogle);
        off.SetActive(!_toogle);
    }
}
