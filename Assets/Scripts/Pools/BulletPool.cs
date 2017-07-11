using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {
    public BulletController prefab;
    public int maxObjects;
    public List<BulletController> inactive;
    public List<BulletController> active;

    void Start () {
        inactive = new List<BulletController>();
        active = new List<BulletController>();
    }

    private int createdObjects() {
        return inactive.Count + active.Count;
    }
    
    public BulletController newObject(Vector3 pos, Quaternion rot) {
        BulletController t = null;
        if (inactive.Count > 0) {
            //Debug.Log("inactive.Count() " + inactive.Count + " > 0");
            t = inactive[0];
            inactive.RemoveAt(0);
            active.Add(t);
        } else if (createdObjects() < maxObjects) {
            //Debug.Log("createdObjects() " + createdObjects() + " < maxObjects " + maxObjects);
            t = Instantiate<BulletController>(prefab, pos, rot);
            t.transform.parent = this.transform;
            active.Add(t);
        }
        if(t == null) {
            throw new System.Exception("Error! Spawned more than the max (" + maxObjects + ") number of bullets.");
        }
        t.init();
        t.gameObject.SetActive(true);
        t.transform.position = pos;
        t.transform.rotation = rot;
        return t;
    }

    // TODO proev forsigtigt om jeg kan goere listene generiske igen
    // og saa proev at udfaktorere de to metoder.

    public void destroyObject(BulletController t) {
        t.gameObject.SetActive(false);
        inactive.Add(t);
        active.Remove(t);
    }
}
