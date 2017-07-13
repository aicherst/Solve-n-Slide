using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter {
    void Collision(float strength);
    void Damage(float amount);
    void AddForce(Vector3 force);
}
