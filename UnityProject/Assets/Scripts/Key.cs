using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    [SerializeField]
    private LineRenderer dashedLine;

    [SerializeField]
    private GameObject door;

    private void Start() {
        if (door == null) {
            Debug.LogWarning("No door assigned => disabled");
            enabled = false;
            return;
        }

        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    // Use this for
    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Manipulation:
                door.SetActive(true);

                if (dashedLine != null) {
                    dashedLine.gameObject.SetActive(true);
                }
                break;
            default:
                if (dashedLine != null) {
                    dashedLine.gameObject.SetActive(false);
                }
                break;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            door.SetActive(false);
        }
    }

    private void Update() {
        if (dashedLine == null)
            return;

        RaycastHit hit;

        Collider doorCollider = door.GetComponent<Collider>();

        if (doorCollider != null && doorCollider.Raycast(new Ray(transform.position, door.transform.position - transform.position), out hit, float.PositiveInfinity)) {
            dashedLine.SetPosition(1, hit.point);
        } else {
            dashedLine.SetPosition(1, door.transform.position);
        }


        dashedLine.SetPosition(0, transform.position);
    }
}
