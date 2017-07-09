using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable {
    Vector3 GetPosition();
    void SetDestroyed(bool destroyed);
}
