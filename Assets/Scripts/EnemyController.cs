using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    private Rigidbody rigidBody;
    private PlayerController player;
    private LevelController playerLevel;
    private LootController lootController;

    public float moveSpeed;
    public int expWorth;
    public int maxHealth;
    public int currentHealth;
    public int damage, hitCooldown;
    public int level; // determines the mob level
    public int lootRolls; // determines the "difficulty/rarity" of the mob

    private Image healthBar;
    private Transform canvasTransform;

    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
        playerLevel = FindObjectOfType<LevelController>();
        lootController = FindObjectOfType<LootController>();

        currentHealth = maxHealth;
        canvasTransform = transform.FindChild("EnemyCanvas");
        healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").
                        FindChild("Health").GetComponent<Image>();

        HurtPlayerManager hurtPlayer = transform.FindChild("Body").GetComponent<HurtPlayerManager>();
        hurtPlayer.setDamage(damage);
        hurtPlayer.setHitCooldown(hitCooldown);
    }
	
	void Update () {
        // if the map will contain height differences, 
        // set the y-value to that of the enemy
        // will probably not work for controller (no intersection check)
        transform.LookAt(player.transform.position);

        if (currentHealth <= 0) {
            die();
        }
        // this billboard stuff doesn't work properly - fix sometime
        canvasTransform.LookAt(Camera.main.transform.position, -Vector3.up);
    }

    private void die() {
        dropLoot();
        Destroy(gameObject);
    }

    private void dropLoot() {
        playerLevel.currentXP += expWorth;
        for (int i = 0; i < lootRolls; i++) {
            Currency c = lootController.roll(level - playerLevel.level);
            if (c != null) {
                float x = c.transform.position.x + transform.position.x;
                float z = c.transform.position.z + transform.position.z;
                c.transform.position = new Vector3(x, -0.9f, z);
                c.itemName = c.type.ToString();
                player.loot.Add(c);
                //Debug.Log("dropped loot! "+c.type);
            } else {
                //Debug.Log("didn't drop loot :(");
            }
        }
    }

    private void FixedUpdate() {
        rigidBody.velocity = transform.forward * moveSpeed;
    }

    public void hurtEnemy(int damage) {
        currentHealth -= damage;
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}

