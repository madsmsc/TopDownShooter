using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour {
    public Currency prefab;
    public int maxObjects;
    public List<Currency> inactive;
    public List<Currency> active;

    void Start () {
        inactive = new List<Currency>();
        active = new List<Currency>();
    }

    private int createdObjects() {
        return inactive.Count + active.Count;
    }
    
    public Currency newObject(Vector3 pos, Quaternion rot) {
        Currency t = null;
        if (inactive.Count > 0) {
            //Debug.Log("inactive.Count() " + inactive.Count + " > 0");
            t = inactive[0];
            inactive.RemoveAt(0);
            active.Add(t);
        } else if (createdObjects() < maxObjects) {
            //Debug.Log("createdObjects() " + createdObjects() + " < maxObjects " + maxObjects);
            t = Instantiate<Currency>(prefab, pos, rot);
            t.transform.parent = this.transform;
            active.Add(t);
        }
        // TODO the exceptions all mention bullets. make them specific to the pools
        if(t == null) {
            throw new System.Exception("Error! Spawned more than the max (" + maxObjects + ") number of bullets.");
        }
        t.init();
        t.gameObject.SetActive(true);
        t.transform.position = pos;
        t.transform.rotation = rot;
        return t;
    }

    public void destroyObject(Currency t) {
        t.gameObject.SetActive(false);
        inactive.Add(t);
        active.Remove(t);
    }
}
