using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerManager: MonoBehaviour {

    private float hitCount, hitCooldown;
    private int damage;

    private void Start() {

    }

    public void setHitCooldown(float f) {
        hitCooldown = f;
    }

    public void setDamage(int i) {
        damage = i;
    }

    public void OnTriggerEnter(Collider other) {
        if (colliderIsPlayer(other)) {
            hurtPlayer(other);
        }
    }

    public void OnTriggerExit(Collider other) {
        if (colliderIsPlayer(other)) {
            hitCount = 0;
        }
    }

    public void OnTriggerStay(Collider other) { 
        if(colliderIsPlayer(other)) {
            hitCount += Time.deltaTime;
            if (hitCount >= hitCooldown) {
                hitCount = 0;
                hurtPlayer(other);
            }
        }
    }

    private void hurtPlayer(Collider other) {
        //Debug.Log("hurt player for " + damage + " damage");
        other.gameObject.GetComponent<PlayerHealthManager>().hurtPlayer(damage);
    }

    private bool colliderIsPlayer(Collider other) {
        return other.gameObject.tag == "Player";
    }
}
