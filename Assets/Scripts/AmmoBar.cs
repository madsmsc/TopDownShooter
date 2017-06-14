using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour {

    public Transform ammoText, weaponText;
    private GunController gun;

	void Start () {
        gun = FindObjectOfType<GunController>();
    }

    // fix different background for the ammo bar
    // maybe use the same background for the skills
    // make a new bar for skills on the right side of the health bar
    // light grey rectangles with rounded edges?
	
	void Update () {
        //transform.position = new Vector3(((float)Screen.width) / 2f, 250f / 3f, 0);

        if (gun.rangedWeapon != null) {
            string loadedAmmo = gun.rangedWeapon.getLoadedAmmo().ToString();
            string unloadedAmmo = gun.rangedWeapon.getUnloadedAmmo().ToString();
            ammoText.GetComponent<Text>().text = loadedAmmo + " / " + unloadedAmmo;
            weaponText.GetComponent<Text>().text = gun.rangedWeapon.gameObject.name;
        } else {
            ammoText.GetComponent<Text>().text = "--";
            if(gun.meleeWeapon != null) {
                weaponText.GetComponent<Text>().text = gun.meleeWeapon.gameObject.name;
            } else {
                weaponText.GetComponent<Text>().text = "--";
            }
        }
    }
}
