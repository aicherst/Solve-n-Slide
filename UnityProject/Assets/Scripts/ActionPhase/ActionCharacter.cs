using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour, ICharacter {
    [SerializeField]
    private int _maxHealth = 100;

    private int _health;

    // Use this for initialization
    void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
            case GamePhase.Manipulation:
                _health = _maxHealth;
                break;
        }
    }

    public int health {
        get {
            return _health;
        }
    }

    public int maxHealth {
        get {
            return _maxHealth;
        }
    }

    public float healthPercentage {
        get {
            return (float)_health / _maxHealth;
        }
    }


    public void Collision(float strength) {
        if (strength > 25) {
            //Debug.Log("collision: " + strength);
            ReduceHealth(10);
        }
        if (strength > 35) {
            //Debug.Log("collision: " + strength);
            ReduceHealth(25);
        }
    }

    public void Damage(float amount) {
        //Debug.Log("Damage: " + amount);
        ReduceHealth((int)amount);
    }

    public void AddForce(Vector3 force) {
        //Debug.Log("Add force");
    }

    private void ReduceHealth(int amount) {
        if (_health <= 0)
            return;

        _health -= amount;

        if (_health <= 0) {
            _health = 0;
            GameStateManager.instance.gamePhase.SetData(GamePhase.Dead);
        } else {
            AudioManager.playLoseHealthSound(transform.position);
        }
    }
}
