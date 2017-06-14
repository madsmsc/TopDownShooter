using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public List<EnemyController> enemies;
    public EnemyController enemy;

	void Start () {
        enemies = new List<EnemyController>();
    }

    void Update() {
        if (enemiesAllDead()) {
            spawnEnemies();
        }
    }

    private bool enemiesAllDead() {
        bool allDead = true;
        for (int i = 0; i < enemies.Count; i++) {
            allDead &= enemies[i] == null;
        }
        return allDead;
    }

    private int spawnTimes = 3;

    private void spawnEnemies() {
        if (spawnTimes <= 0)
            return;
        spawnTimes--;

        enemies.Clear();
        for (int i = 0; i < 4; i++) {
            Vector3 pos = gameObject.transform.position;
            pos.x += i;
            pos.y += 1;
            EnemyController e = Instantiate<EnemyController>(enemy, pos, Quaternion.identity);
            enemies.Add(e);
            //Debug.Log("creating enemy " + i);
        }
    }
}
