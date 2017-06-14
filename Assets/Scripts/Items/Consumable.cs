using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item {

    // s
    public float cooldown, power, charges;
    // s
    private float timeSinceUse;

    void Start () {
		
	}
	
	void Update () {
        if (timeSinceUse < cooldown) {
            timeSinceUse += Time.deltaTime;
        }
    }
    
    public void use() {
        timeSinceUse = 0;
    }

    public float powerPrSec() {
        if (cooldown == 0)
            return 0;
        return power / cooldown;
    }

    public bool offCooldown() {
        return timeSinceUse >= cooldown;
    }
}