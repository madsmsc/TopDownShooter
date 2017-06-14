using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
    // points, attacks/s
    public float damage, attackCooldown;
    public List<WeaponMod> mods;
    private float timeSinceAttack;

    public float realDamage() {
        float realDamage = damage;
        for(int i = 0; i < mods.Count; i++) {
            realDamage += mods[i].damage;
        }
        return realDamage;
    }

    public float realAttackCooldown() {
        float realAttackCooldown = attackCooldown;
        for (int i = 0; i < mods.Count; i++) {
            realAttackCooldown += mods[i].attackCooldown;
        }
        return realAttackCooldown;
    }

    void Start() {
        mods = new List<WeaponMod>();
    }

    void Update() {
        if(timeSinceAttack < realAttackCooldown()) {
            timeSinceAttack += Time.deltaTime;
        }
    }

    public void attack() {
        timeSinceAttack = 0;
    }

    public bool offCooldown() {
        return timeSinceAttack >= realAttackCooldown();
    }

    public float dps() {
        if (realAttackCooldown() == 0)
            return 0;
        return realDamage() / realAttackCooldown();
    }
}