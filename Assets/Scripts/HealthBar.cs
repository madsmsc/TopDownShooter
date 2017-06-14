using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Transform armorBar, powerBar, xpBar, text;
    private PlayerHealthManager playerHealth;
    private LevelController levelController;

	void Start () {
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        levelController = FindObjectOfType<LevelController>();
    }

	void Update () {
        //transform.position = new Vector3 (((float)Screen.width) / 2f, 280f / 3f, 0);

        string playerLevelText = "lvl " + levelController.level;
        text.GetComponent<Text>().text = playerLevelText;

        float armorBarFill = (float)playerHealth.currentHealth / (float)playerHealth.maxHealth / 2f;
        armorBar.GetComponent<Image>().fillAmount = armorBarFill;

        float powerBarFill = (float)playerHealth.currentPower / (float)playerHealth.maxPower / 2f;
        powerBar.GetComponent<Image>().fillAmount = powerBarFill;

        float xpBarFill = (float)levelController.currentXP / (float)levelController.maxXP;
        xpBar.GetComponent<Image>().fillAmount = xpBarFill;

        //Debug.Log("armor="+player.currentHealth+", power="+player.currentPower);
    }
}
