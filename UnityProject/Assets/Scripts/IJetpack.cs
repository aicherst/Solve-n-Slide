using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJetpack {
    void Thrust(bool enable);

    void MovementInput(Vector2 input);

    float maxFuel {
        get;
    }

    float fuel {
        get;
        set;
    }
}
