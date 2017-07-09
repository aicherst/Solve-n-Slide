using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Spawn spawn;

    public GameObject manipulationCharacter;
    public GameObject actionCharacter;

    private static Property<Player> _mainPlayer = new Property<Player>();

    public static Property<Player> mainPlayer {
        get {
            return _mainPlayer;
        }
    }

    private void Start() {
        actionCharacter = spawn.InstantiateCharacter();

        _mainPlayer.SetData(this);
    }

    public T GetCharacterComponent<T>() {
        T component;

        component = manipulationCharacter.GetComponent<T>();

        if (component != null) {
            return component;
        }


        component = actionCharacter.GetComponent<T>();

        if (component != null) {
            return component;
        }


        component = manipulationCharacter.GetComponentInChildren<T>();

        if (component != null) {
            return component;
        }

        return actionCharacter.GetComponentInChildren<T>();
    }
}
