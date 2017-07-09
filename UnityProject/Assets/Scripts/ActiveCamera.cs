using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCamera {
    private static Property<Camera> _camera = new Property<Camera>();

    public static Property<Camera> camera {
        get {
            return _camera;
        }
    }
}
