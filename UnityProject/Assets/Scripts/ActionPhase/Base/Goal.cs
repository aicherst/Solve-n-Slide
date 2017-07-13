using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public GameObject fireworks;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.player)) {
            AudioManager.PlayReachedGoalSound(transform.position);

            fireworks.SetActive(true);

            GameStateManager.instance.gamePhase.SetData(GamePhase.Finished);
        }
    }
}
