using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    public EnemyController prefab;
    public int maxObjects;
    public List<EnemyController> inactive;
    public List<EnemyController> active;

    void Start () {
        inactive = new List<EnemyController>();
        active = new List<EnemyController>();
    }

    private int createdObjects() {
        return inactive.Count + active.Count;
    }
    
    public EnemyController newObject(Vector3 pos, Quaternion rot) {
        EnemyController t = null;
        if (inactive.Count > 0) {
            //Debug.Log("inactive.Count() " + inactive.Count + " > 0");
            t = inactive[0];
            inactive.RemoveAt(0);
            active.Add(t);
        } else if (createdObjects() < maxObjects) {
            //Debug.Log("createdObjects() " + createdObjects() + " < maxObjects " + maxObjects);
            t = Instantiate<EnemyController>(prefab, pos, rot);
            t.transform.parent = this.transform;
            active.Add(t);
        }
        if(t == null) {
            throw new System.Exception("Error! Spawned more than the max (" + maxObjects + ") number of bullets.");
        }
        t.enemyPool = this;
        t.gameObject.SetActive(true);
        t.transform.position = pos;
        t.transform.rotation = rot;
        return t;
    }

    public void destroyObject(EnemyController t) {
        t.gameObject.SetActive(false);
        inactive.Add(t);
        active.Remove(t);
    }
}
