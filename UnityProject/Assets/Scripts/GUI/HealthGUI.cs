using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGUI : MonoBehaviour {
    public Image healthBarLeft, healthBarRight;

    public Color lerpToColor = Color.red;

    public Text healthText;


    private Color baseColor;

    private static readonly float BAR_WIDTH = 209f;

    private ActionCharacter actionCharacter;

    private void Start() {
        baseColor = healthBarLeft.color;

        Player.mainPlayer.AddListener(OnMainPlayerChange);
    }

    private void OnMainPlayerChange(ReadOnlyProperty<Player> changedProperty, Player newData, Player oldData) {
        actionCharacter = null;

        if (newData != null) {
            actionCharacter = newData.GetCharacterComponent<ActionCharacter>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (actionCharacter == null)
            return;

        float healthPercentage = actionCharacter.healthPercentage;
        healthBarLeft.transform.localPosition = new Vector3(BAR_WIDTH * (1f - healthPercentage), 0);
        healthBarRight.transform.localPosition = new Vector3(-BAR_WIDTH * (1f - healthPercentage), 0);

        healthText.text = actionCharacter.health.ToString();

        healthBarLeft.color = Color.Lerp(lerpToColor, baseColor, healthPercentage);
        healthBarRight.color = Color.Lerp(lerpToColor, baseColor, healthPercentage);
    }
}
