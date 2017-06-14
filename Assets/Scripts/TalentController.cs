using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentController : MonoBehaviour {

    public enum Talent { armor1, armor2, health1, health2, ammo1, ammo2, ammo3 };
    public List<Talent> talents = new List<Talent>();
     
    public int healthModifier() {
        int modifier = 0;
        foreach(Talent t in talents) {
            if (t == Talent.health1)
                modifier += 10;
            if (t == Talent.health2)
                modifier += 5;
        }
        return modifier;
    }

    public int armorModifier() {
        int modifier = 0;
        foreach (Talent t in talents) {
            if (t == Talent.armor1)
                modifier += 10;
            if (t == Talent.armor2)
                modifier += 5;
        }
        return modifier;
    }

    public int ammoModifier() {
        int modifier = 0;
        foreach (Talent t in talents) {
            if (t == Talent.ammo1)
                modifier += 1;
            if (t == Talent.ammo2)
                modifier += 1;
        }
        return modifier;
    }

    public int ammoDamageModifier() {
        int modifier = 0;
        foreach (Talent t in talents) {
            if (t == Talent.ammo3)
                modifier += 1;
        }
        return modifier;
    }

    void Start () {
		
	}
}