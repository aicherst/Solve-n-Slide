using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCube : MonoBehaviour {
    [SerializeField]
    private MeshRenderer preview;

    private string _levelName;

    public string levelName {
        get {
            return _levelName;
        }
        set {
            _levelName = value;
        }
    }

    public void SetPreviewTexture(Texture2D previewTexture) {
        preview.material.SetTexture("_EmissionMap", previewTexture);
    }
}
