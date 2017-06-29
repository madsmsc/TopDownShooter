using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: make a general HealthManager class that applies 
//       to both enemies, the player and eventual NPCs.

public class PlayerHealthManager : MonoBehaviour {

    public float flashLength;
    private float flashCount;
    private float regenCount;

    private Renderer render;
    private Color color;

    public int maxHealth;
    public int currentHealth;
    public int healthPerSec;

    public int maxPower;
    public int currentPower;
    public int powerPerSec;

    void Start() {
        currentHealth = maxHealth;
        currentPower = maxPower;
        render = GetComponent<Renderer>();
        color = render.material.GetColor("_Color");
    }

    void Update() {
        // it's all been moved to fixedUpdate
    }

    void FixedUpdate () {
        checkDead();
        checkFlash();
        regenStats();
        capStats();
	}

    private void regenStats() {
        regenCount += Time.deltaTime;
        if (regenCount >= 1) {
            currentHealth += healthPerSec;
            currentPower += powerPerSec;
            regenCount = 0;
        }
    }

    private void capStats() {
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentPower > maxPower)
            currentPower = maxPower;
    }

    private void checkDead() {
        if (currentHealth <= 0) {
            gameObject.SetActive(false);
        }
    }

    private void checkFlash() {
        if (flashCount > 0) {
            flashCount -= Time.deltaTime;
            if (flashCount <= 0) {
                render.material.SetColor("_Color", color);
            }
        }
    }

    public void hurtPlayer(int damage) {
        //Debug.Log("Player hurt for " + damage + " damage.");
        currentHealth -= damage;
        flashCount = flashLength;
        render.material.SetColor("_Color", Color.white);
    }
}
