using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damage;
    public float speed;
    public float lifetime;
    public BulletPool bulletPool;
    private float ttl;

    public void init() {
        ttl = lifetime;
    }

	void Start () {
        init();
    }

	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // fix pooling system as well as timer... 
        ttl -= Time.deltaTime;
        if(ttl <= 0) {
            bulletPool.destroy(this);
        }
	}

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().hurtEnemy(damage);
            bulletPool.destroy(this);
        }
    }
}
