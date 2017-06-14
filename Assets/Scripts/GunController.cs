using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public BulletController bullet;
    public RangedWeapon rangedWeapon;
    public MeleeWeapon meleeWeapon;
    public Transform firePoint;

    public bool isFiring;
    public bool secondaryFireMode;
    public float bulletSpeed;
    public float timeBetweenShots;
    
    private float shotCount; // countdown for timeBetweenShots

    void Start () {
		
	}

    void Update() {
        if (shotCount >= 0) {
            shotCount -= Time.deltaTime;
        }
        if (!isFiring) {
            //Debug.Log("isn't firing");
            return;
        }
        if (rangedWeapon == null) {
            //Debug.Log("no weapon equipped");
            return; 
        }
        if(rangedWeapon.getLoadedAmmo() == 0 && rangedWeapon.getUnloadedAmmo() == 0) {
            //Debug.Log("no bullets left");
            return; 
        }
        decideReload();
        decideShoot();
    }

    public int getDamage() {
        return (int) rangedWeapon.damage;
    }

    private void decideReload() {
        if (rangedWeapon.getLoadedAmmo() == 0) {
            //Debug.Log("Realoading");
            int ammoLeft = rangedWeapon.capacity;
            if(rangedWeapon.getUnloadedAmmo() < ammoLeft) {
                ammoLeft = rangedWeapon.getUnloadedAmmo();
            }
            rangedWeapon.addLoadedAmmo(ammoLeft);
            rangedWeapon.addUnloadedAmmo(- ammoLeft);
        }
    }

    private void decideShoot() {
        if (shotCount <= 0) {
            //Debug.Log("Shooting");
            if (secondaryFireMode) {
                shootSecondary();
            } else {
                shootMain();
            }
        }
    }

    private void shootMain() {
        rangedWeapon.addLoadedAmmo(-1);
        shotCount = timeBetweenShots;
        newBullet(firePoint.rotation);
    }

    private void shootSecondary() {
        if(rangedWeapon.getLoadedAmmo() == 1) {
            shootMain();
        } else if(rangedWeapon.getLoadedAmmo() == 2) {
            rangedWeapon.addLoadedAmmo(-2);
            shotCount = timeBetweenShots * 2;
            float angle = 5f / 360f;
            Quaternion rotLeft = firePoint.rotation;
            rotLeft.y -= angle;
            Quaternion rotRight = firePoint.rotation;
            rotRight.y += angle;

            newBullet(rotLeft);
            newBullet(rotRight);
        } else {
            rangedWeapon.addLoadedAmmo(-3);
            shotCount = timeBetweenShots * 2;
            float angle = 10f / 360f;
            Quaternion rotLeft = firePoint.rotation;
            rotLeft.y -= angle;
            Quaternion rotRight = firePoint.rotation;
            rotRight.y += angle;

            newBullet(rotLeft);
            newBullet(firePoint.rotation);
            newBullet(rotRight);
        }
    }

    private void newBullet(Quaternion rotation) {
        BulletController b = Instantiate<BulletController>(bullet, firePoint.position, rotation);
        b.speed = bulletSpeed;
        b.damage = getDamage();
    }

    public void mainAttackDown() {
        isFiring = true;
    }

    public void mainAttackUp() {
        isFiring = false;
    }

    public void secondaryAttackDown() {
        isFiring = true;
        secondaryFireMode = true;
    }

    public void secondaryAttackUp() {
        isFiring = false;
        secondaryFireMode = false;
    }
}
