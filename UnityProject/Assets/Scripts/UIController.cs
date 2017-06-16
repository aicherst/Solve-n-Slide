using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    //public Text healthText;
    //public Image healthBackground;
    //public Text fuelText;
    //public Image fuelBackground;
    //public Text chargesText;
    //public Image chargesBackground;
    //public Text levelFinishText;
    //public Image levelFinishBackground;
    //public Text deathText;
    //public Image deathBackground;
    //public Text infoText;
    public Player player;

    public Image crosshair;

    public Image fuelHealthForeground;
    public Image fuelBarLeft, fuelBarRight, healthBarLeft, healthBarRight;
    private float fuelBarLeftOffset, fuelBarRightOffset, healthBarLeftOffset, healthBarRightOffset;
    public Text fuelText, healthText;

    private const float barWidth = 209f;

    private Image[] fuelTankIcons;
    public Sprite fuelTankIconSprite, fuelTankIconSpriteBW;

    private Image[] chargeIcons;
    public Sprite chargeIconSprite, chargeIconSpriteBW;

    // Use this for initialization
    void Start() {
        fuelBarLeftOffset = fuelBarLeft.transform.position.x;
        fuelBarRightOffset = fuelBarRight.transform.position.x;
        healthBarLeftOffset = healthBarLeft.transform.position.x;
        healthBarRightOffset = healthBarRight.transform.position.x;
        chargeIcons = new Image[player.getManipulationCharacter().getMaxCharges()];
        for (int i = 0; i < chargeIcons.Length; i++) {
            GameObject go = new GameObject();
            chargeIcons[i] = go.AddComponent<Image>();
            chargeIcons[i].sprite = chargeIconSprite;
            chargeIcons[i].rectTransform.anchorMin = new Vector2(0.5f, 0);
            chargeIcons[i].rectTransform.anchorMax = new Vector2(0.5f, 0);
            chargeIcons[i].rectTransform.pivot = new Vector2(0.5f, 0.5f);
            chargeIcons[i].transform.SetParent(GetComponent<Canvas>().transform);
            chargeIcons[i].rectTransform.localScale = new Vector3(0.5f, 0.5f);
            chargeIcons[i].rectTransform.anchoredPosition = new Vector3(30-(chargeIcons.Length*60)/2+i*60, 35);
        }

        fuelTankIcons = new Image[ManipulationCharacter.getMaxFuelTanks()];
        for (int i = 0; i < fuelTankIcons.Length; i++) {
            GameObject go = new GameObject();
            fuelTankIcons[i] = go.AddComponent<Image>();
            fuelTankIcons[i].sprite = fuelTankIconSprite;
            fuelTankIcons[i].rectTransform.anchorMin = new Vector2(0.5f, 0);
            fuelTankIcons[i].rectTransform.anchorMax = new Vector2(0.5f, 0);
            fuelTankIcons[i].rectTransform.pivot = new Vector2(0.5f, 0.5f);
            fuelTankIcons[i].transform.SetParent(GetComponent<Canvas>().transform);
            fuelTankIcons[i].rectTransform.localScale = new Vector3(0.5f, 0.5f);
            fuelTankIcons[i].rectTransform.anchoredPosition = new Vector3(30 - (fuelTankIcons.Length * 60) / 2 + i * 60, 90);
        }
    }

    // Update is called once per frame
    void Update() {
        bool fuelHealthEnabled = player.getCurrentPhase() == Player.Phase.ACTION_PHASE;
        fuelHealthForeground.enabled = fuelHealthEnabled;
        foreach(Image child in fuelHealthForeground.GetComponentsInChildren<Image>()) {
            child.enabled = fuelHealthEnabled;
        }
        foreach (Text child in fuelHealthForeground.GetComponentsInChildren<Text>()) {
            child.enabled = fuelHealthEnabled;
        }

        bool manipulationUIEnabled = player.getCurrentPhase() == Player.Phase.MANIPULATION_PHASE;
        for (int i = 0; i < chargeIcons.Length; i++) {
            chargeIcons[i].enabled = manipulationUIEnabled;
        }
        for (int i = 0; i < fuelTankIcons.Length; i++) {
            fuelTankIcons[i].enabled = manipulationUIEnabled;
        }
        if (player.getCurrentPhase() == Player.Phase.ACTION_PHASE) {
            float fuel = player.getActionCharacter().getFuel();
            float maxFuel = player.getActionCharacter().getMaxFuel();
            float health = player.getActionCharacter().getHealth();
            float maxHealth = player.getActionCharacter().getMaxHealth();
            fuelText.text = makeThreeChars(Mathf.RoundToInt(fuel));
            healthText.text = makeThreeChars(player.getActionCharacter().getHealth());

            fuelBarLeft.transform.position = new Vector3(fuelBarLeftOffset + barWidth * (1f-(fuel/maxFuel)), fuelBarLeft.transform.position.y);
            fuelBarRight.transform.position = new Vector3(fuelBarRightOffset - barWidth * (1f - (fuel / maxFuel)), fuelBarLeft.transform.position.y);
            healthBarLeft.transform.position = new Vector3(healthBarLeftOffset + barWidth * (1f - (health / maxHealth)), healthBarLeft.transform.position.y);
            healthBarRight.transform.position = new Vector3(healthBarRightOffset - barWidth * (1f - (health / maxHealth)), healthBarLeft.transform.position.y);

            fuelBarLeft.color = new Color32((byte)(255 - fuel / maxFuel * 255), (byte)(fuel / maxFuel * 255), 0, 150);
            fuelBarRight.color = new Color32((byte)(255 - fuel / maxFuel * 255), (byte)(fuel / maxFuel * 255), 0, 150);
            healthBarLeft.color = new Color32((byte)(255 - health / maxHealth * 255), (byte)(health / maxHealth * 255), 0, 150);
            healthBarRight.color = new Color32((byte)(255 - health / maxHealth * 255), (byte)(health / maxHealth * 255), 0, 150);
        } else if (player.getCurrentPhase() == Player.Phase.MANIPULATION_PHASE) {
            for(int i = 0; i < chargeIcons.Length; i++) {
                if(i < player.getManipulationCharacter().getCharges()) {
                    chargeIcons[i].sprite = chargeIconSprite;
                } else {
                    chargeIcons[i].sprite = chargeIconSpriteBW;
                }
            }
            for (int i = 0; i < fuelTankIcons.Length; i++) {
                if (i < ManipulationCharacter.getFuelTanks()) {
                    fuelTankIcons[i].sprite = fuelTankIconSprite;
                } else {
                    fuelTankIcons[i].sprite = fuelTankIconSpriteBW;
                }
            }
        }


        //healthText.enabled = false;
        //healthBackground.enabled = false;
        //fuelText.enabled = false;
        //fuelBackground.enabled = false;
        //chargesText.enabled = false;
        //chargesBackground.enabled = false;
        //levelFinishText.enabled = false;
        //levelFinishBackground.enabled = false;
        //deathText.enabled = false;
        //deathBackground.enabled = false;
        //crosshair.enabled = false;
        //infoText.enabled = false;

        //if (player.getCurrentPhase() == Player.Phase.MANIPULATION_PHASE) {
        //    chargesText.enabled = true;
        //    chargesBackground.enabled = true;
        //    chargesText.text = "Test Charges: " + player.getManipulationCharacter().getCharges() + "/" + player.getManipulationCharacter().getMaxCharges();
        //    crosshair.enabled = true;
        //    infoText.enabled = true;
        //    infoText.text = "Press Enter to switch to Action-Phase";
        //} else if (player.getCurrentPhase() == Player.Phase.ACTION_PHASE) {
        //    if (player.getActionCharacter().getLevelFinished()) {
        //        levelFinishBackground.enabled = true;
        //        levelFinishText.enabled = true;
        //    } else if (player.getActionCharacter().getDead()) {
        //        deathBackground.enabled = true;
        //        deathText.enabled = true;
        //    } else {
        //        healthText.enabled = true;
        //        healthBackground.enabled = true;
        //        healthText.text = "Test Health: " + makeThreeChars(player.getActionCharacter().getHealth()) + "/"
        //            + makeThreeChars(player.getActionCharacter().getMaxHealth());

        //        fuelText.enabled = true;
        //        fuelBackground.enabled = true;
        //        fuelText.text = "Test Fuel: " + makeThreeChars(Mathf.RoundToInt(player.getActionCharacter().getFuel())) + "/"
        //            + makeThreeChars(Mathf.RoundToInt(player.getActionCharacter().getMaxFuel()));

        //        infoText.enabled = true;
        //        infoText.text = "Press Enter to switch to Manipulation-Phase";
        //    }
        //}
    }

    private string makeThreeChars(int value) {
        string valueString;
        //if (value < 10)
        //    valueString = "00" + value;
        //else if (value < 100)
        //    valueString = "0" + value;
        //else
            valueString = value.ToString();
        return valueString;
    }
}
