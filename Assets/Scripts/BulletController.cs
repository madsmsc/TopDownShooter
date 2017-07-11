using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour {
    public int damage;
    public float speed;
    public float lifetime;
    public BulletPool pool;

    private float ttl;

    public void init() {
        ttl = lifetime;
    }

    void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        ttl -= Time.deltaTime;
        if(ttl <= 0) {
            pool.destroyObject(this);
        }
	}

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().hurtEnemy(damage);
            pool.destroyObject(this);
        }
    }
}
