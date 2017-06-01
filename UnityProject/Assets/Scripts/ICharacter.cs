using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter {
    void collision(float strength);
    void damage(float amount);
    void addForce(Vector3 force);
}
