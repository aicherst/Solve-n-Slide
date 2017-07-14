using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHelp : MonoBehaviour {
    public Text toolTip, enterForHelp;

    private float fadeTime = 0.3f;

    void Start() {
        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    public void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase newData, GamePhase oldData) {
        switch (newData) {
            case GamePhase.Action:
                enterForHelp.CrossFadeAlpha(1, fadeTime, true);
                toolTip.CrossFadeAlpha(0, fadeTime, true);
                break;
            case GamePhase.Manipulation:
                enterForHelp.CrossFadeAlpha(0, fadeTime, true);
                toolTip.CrossFadeAlpha(1, fadeTime, true);
                break;
        }
    }

    private void OnDestroy() {
        GameStateManager.instance.gamePhase.RemoveListener(OnGamePhaseChange);
    }
}
