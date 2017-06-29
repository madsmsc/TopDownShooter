using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {
    public BulletController bulletPrefab;
    public int maxBullets;

    public int createdBullets = 0;
    public List<BulletController> inactiveBullets;

    void Start () {
        inactiveBullets = new List<BulletController>(maxBullets);
    }

    // TODO the bullet pool isn't working. I'm just creating and killing bullets now.

    public BulletController newBullet(Vector3 position, Quaternion rotation) {
        BulletController bullet = Instantiate<BulletController>(bulletPrefab, position, rotation);
        bullet.transform.parent = this.transform;
        return bullet;
        /*
        BulletController bullet;
        if (inactiveBullets.Count > 0) {
            bullet = inactiveBullets[0];
            inactiveBullets.RemoveAt(0);
        } else if (createdBullets < maxBullets) {
            bullet = Instantiate<BulletController>(bulletPrefab, position, rotation);
            createdBullets++;
        } else {
            Debug.Log("Error! Spawned more than the max (" + maxBullets + ") number of bullets.");
            return null;
        }
        bullet.bulletPool = this;
        bullet.gameObject.SetActive(true);
        bullet.transform.parent = this.transform;
        return bullet;
        */
    }

    public void destroy(BulletController bullet) {
        Destroy(bullet.gameObject);
        Destroy(bullet);
        /*
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = null;
        inactiveBullets.Add(bullet);
        */
    }
}
