using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public int level, currentXP, maxXP;
    public float scaleNextLevelXP;

	void Start () {
		
	}
	
	void Update () {
        checkLevelUp();
	}

    public void checkLevelUp() {
        if(currentXP >= maxXP) {
            level += 1;
            currentXP = currentXP - maxXP;
            maxXP = (int) (maxXP * scaleNextLevelXP);
        }
    }
}
