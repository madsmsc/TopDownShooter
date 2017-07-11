using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPool : MonoBehaviour {
    public GreenController prefab;
    public int maxObjects;
    public List<GreenController> inactive;
    public List<GreenController> active;

    void Start () {
        inactive = new List<GreenController>();
        active = new List<GreenController>();
    }

    private int createdObjects() {
        return inactive.Count + active.Count;
    }
    
    public GreenController newObject(Vector3 pos, Quaternion rot) {
        GreenController t = null;
        if (inactive.Count > 0) {
            //Debug.Log("inactive.Count() " + inactive.Count + " > 0");
            t = inactive[0];
            inactive.RemoveAt(0);
            active.Add(t);
        } else if (createdObjects() < maxObjects) {
            //Debug.Log("createdObjects() " + createdObjects() + " < maxObjects " + maxObjects);
            t = Instantiate<GreenController>(prefab, pos, rot);
            t.transform.parent = this.transform;
            active.Remove(t);
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

    public void destroyObject(GreenController t) {
        t.gameObject.SetActive(false);
        inactive.Add(t);
        active.Remove(t);
    }
}
