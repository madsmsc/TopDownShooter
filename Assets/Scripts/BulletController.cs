using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damage;
    public float speed;
    public float lifetime;

	void Start () {
	}

	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // fix pooling system as well as timer... 
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().hurtEnemy(damage);
            Destroy(gameObject);
        }
    }
}
