using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {
    public BulletController bulletPrefab;
    public int maxBullets;
    public int createdBullets = 0;
    public int inactiveCount;
    public BulletController[] inactiveBullets;

    void Start () {
        inactiveBullets = new BulletController[maxBullets];
    }
    
    public BulletController newBullet(Vector3 position, Quaternion rotation) {
        BulletController bullet = null;
        if (inactiveCount > 0) {
            //Debug.Log("inactiveCount (" + inactiveCount + ") > 0");
            bullet = inactiveBullets[inactiveCount - 1];
            inactiveBullets[inactiveCount - 1] = null;
            inactiveCount--;
        } else if (createdBullets < maxBullets) {
            //Debug.Log("createdBullets (" + createdBullets + ") < maxBullets (" + maxBullets);
            bullet = Instantiate<BulletController>(bulletPrefab, position, rotation);
            bullet.bulletPool = this;
            bullet.transform.parent = this.transform;
            createdBullets++;
        }
        if(bullet == null) {
            throw new System.Exception("Error! Spawned more than the max (" + maxBullets + ") number of bullets.");
        }
        bullet.init();
        bullet.gameObject.SetActive(true);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        return bullet;
    }

    public void destroy(BulletController bullet) {
        bullet.gameObject.SetActive(false);
        inactiveBullets[inactiveCount] = bullet;
        inactiveCount++;
    }
}
