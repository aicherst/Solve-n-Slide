using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.player)) {
            GameStateManager.instance.gamePhase.SetData(GamePhase.Finished);
        }
    }
}
