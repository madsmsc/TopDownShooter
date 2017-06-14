using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour {

    private PlayerHealthManager playerHealth;
    private GunController gun;
    private NotificationController notifications;
    private List<string> skills;

    private string MATERIALIZE_AMMO = "Materialize Ammo";
    private string NANOBOTS = "Nanobots";
    private string HACK = "Hack";
    private string EMP = "EMP";
    private string OVERDRIVE = "Overdrive";
    private string GRENADE = "Grenade";

    private int NUMBER_OF_SKILLS = 9;

    private string USING_SKILL = "Using ";
    private string NO_POWER = "Not enough power to use ";

    // TODO MPE: smid det ud i objekter og goer koden generel

    void Start () {
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        gun = FindObjectOfType<GunController>();
        notifications = FindObjectOfType<NotificationController>();

        skills = new List<string>();
        for(int i = 0; i < NUMBER_OF_SKILLS; i++) {
            skills.Add("");
        }
        skills[1] = MATERIALIZE_AMMO;
        skills[2] = NANOBOTS;
        skills[3] = HACK;
        skills[4] = EMP;
        skills[5] = OVERDRIVE;
        skills[6] = GRENADE;
    }
	
	void Update () {
		
	}

    private void notify(string skill, bool use) {
        notifications.showNotification((use ? USING_SKILL : NO_POWER) + skill);
    }

    public void useSkill(int i) {
        //Debug.Log("useSkill (" + i + " )");
        if(i < 1 || i > NUMBER_OF_SKILLS || skills[i] == "") {
            //Debug.Log("no such skill");
        } else {
            //Debug.Log("Using skill: " + skills[i]);
            if (skills[i] == MATERIALIZE_AMMO) {
                materializeAmmo();
            } else if (skills[i] == NANOBOTS) {
                nanobots();
            } else if (skills[i] == HACK) {
                hack();
            } else if (skills[i] == EMP) {
                emp();
            } else if (skills[i] == OVERDRIVE) {
                overdrive();
            } else if (skills[i] == GRENADE) {
                grenade();
            }
        }
    }

    private void materializeAmmo() {
        int cost = 60;
        bool use = playerHealth.currentPower >= cost;
        if (use) {
            playerHealth.currentPower -= cost;
            gun.rangedWeapon.maxUnloadedAmmo();
            gun.rangedWeapon.maxLoadedAmmo();
        }
        notify(MATERIALIZE_AMMO, use);
    }

    private void nanobots() {
        int cost = 50;
        int healAmount = 50;
        bool use = playerHealth.currentPower >= cost;
        if (use) {
            playerHealth.currentPower -= cost;
            playerHealth.currentHealth += healAmount;
        }
        notify(NANOBOTS, use);
    }

    private void hack() {
        int cost = 45;
        bool use = playerHealth.currentPower >= cost;
        if (use) {
            playerHealth.currentPower -= cost;
        }
        notify(HACK, use);
    }

    private void emp() {
        int cost = 55;
        bool use = playerHealth.currentPower >= cost;
        if (use) {
            playerHealth.currentPower -= cost;
        }
        notify(EMP, use);
    }

    private void overdrive() {
        int cost = 35;
        bool use = playerHealth.currentPower >= cost;
        if (use) {
            playerHealth.currentPower -= cost;
        }
        notify(OVERDRIVE, use);
    }

    private void grenade() {
        int cost = 35;
        bool use = playerHealth.currentPower >= cost;
        if (use) {
            playerHealth.currentPower -= cost;
        }
        notify(GRENADE, use);
    }
}
