using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMod : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    // attaches to melee/ranged weapon
    public bool meleeMod, rangedMod;
    // ranged multipliers (percent)
    public float reloadTime, projectileSpeed, capacity;
    // melee multipliers (percent)
    public float aoeRange, damage, attackCooldown;

    public WeaponMod spikes() {
        meleeMod = true;
        damage = 1.1f;
        return this;
    }

    public WeaponMod silencer() {
        rangedMod = true;
        projectileSpeed = 1.1f;
        return this;
    }

    public WeaponMod extendedMag() {
        rangedMod = true;
        capacity = 1.2f;
        return this;
    }
}