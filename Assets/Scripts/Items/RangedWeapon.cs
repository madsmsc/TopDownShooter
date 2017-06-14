using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon {

    // TODO 1: find ud af hvordan man laver fancy getter/setter i csharp
    // TODO 2: tjek alle klasser og soerg for at det kun er strengt noedvendige 
    //         felter som er public. alt andet skal vaere private.

    // s, m/s
    public float reloadTime, projectileSpeed;
    // number of bullets
    public int capacity;
    private int loadedAmmo, unloadedAmmo;

    public int getLoadedAmmo() {
        return loadedAmmo;
    }

    public int getUnloadedAmmo() {
        return unloadedAmmo;
    }

    public void addLoadedAmmo(int add) {
        loadedAmmo += add;
    }

    public void addUnloadedAmmo(int add) {
        unloadedAmmo += add;
    }

    public void setLoadedAmmo(int set) {
        loadedAmmo = set;
    }

    public void setUnloadedAmmo(int set) {
        unloadedAmmo = set;
    }

    public void maxLoadedAmmo() {
        loadedAmmo = capacity;
    }

    // you start with 10 mags
    public void maxUnloadedAmmo() {
        unloadedAmmo = capacity * 9;
    }

    void Start () {
        maxLoadedAmmo();
        maxUnloadedAmmo();
	}
	
	void Update () {
		
	}
}