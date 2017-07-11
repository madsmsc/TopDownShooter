using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public RangedWeapon equippedWeapon;
    public Transform firePoint;
    public BulletPool bulletPool;
    public RangedWeapon primaryWeapon, secondaryWeapon;

    public bool isFiring;
    public bool secondaryFireMode;
    
    private float shotCount; // countdown for timeBetweenShots

    void Start () {
        equippedWeapon = secondaryWeapon;
        secondaryWeapon.gameObject.SetActive(true);
        primaryWeapon.gameObject.SetActive(false);
    }

    public void switchWeapon() {
        //Debug.Log("Switch weapon!");
        if (equippedWeapon == secondaryWeapon) {
            equippedWeapon = primaryWeapon;
            secondaryWeapon.gameObject.SetActive(false);
            primaryWeapon.gameObject.SetActive(true);
        } else {
            equippedWeapon = secondaryWeapon;
            secondaryWeapon.gameObject.SetActive(true);
            primaryWeapon.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (shotCount >= 0) {
            shotCount -= Time.deltaTime;
        }
        if (!isFiring) {
            //Debug.Log("isn't firing");
            return;
        }
        if (equippedWeapon == null) {
            //Debug.Log("no weapon equipped");
            return; 
        }
        if(equippedWeapon.getLoadedAmmo() == 0 && equippedWeapon.getUnloadedAmmo() == 0) {
            //Debug.Log("no bullets left");
            return; 
        }
        decideReload();
        decideShoot();
    }

    public int getDamage() {
        return (int) equippedWeapon.damage;
    }

    public int getBulletSpeed() {
        return (int) equippedWeapon.projectileSpeed;
    }

    public float timeBetweenShots() {
        return equippedWeapon.attackCooldown;
    }

    private void decideReload() {
        if (equippedWeapon.getLoadedAmmo() == 0) {
            //Debug.Log("Realoading");
            int ammoLeft = equippedWeapon.capacity;
            if(equippedWeapon.getUnloadedAmmo() < ammoLeft) {
                ammoLeft = equippedWeapon.getUnloadedAmmo();
            }
            equippedWeapon.addLoadedAmmo(ammoLeft);
            equippedWeapon.addUnloadedAmmo(- ammoLeft);
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
        equippedWeapon.addLoadedAmmo(-1);
        shotCount = timeBetweenShots();
        newBullet(firePoint.rotation);
    }

    private void shootSecondary() {
        if(equippedWeapon.getLoadedAmmo() == 1) {
            shootMain();
        } else if(equippedWeapon.getLoadedAmmo() == 2) {
            equippedWeapon.addLoadedAmmo(-2);
            shotCount = timeBetweenShots() * 2;
            float angle = 5f / 360f;
            Quaternion rotLeft = firePoint.rotation;
            rotLeft.y -= angle;
            Quaternion rotRight = firePoint.rotation;
            rotRight.y += angle;

            newBullet(rotLeft);
            newBullet(rotRight);
        } else {
            equippedWeapon.addLoadedAmmo(-3);
            shotCount = timeBetweenShots() * 2;
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
        BulletController b = bulletPool.newBullet(firePoint.position, rotation);
        b.speed = getBulletSpeed();
        b.damage = getDamage();
        b.bulletPool = bulletPool;
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
