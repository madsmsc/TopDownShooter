using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectSpawner : MonoBehaviour{
    public GameObject enemiesNode;
    public GameObject worldObjectsNode;
    [Range(0, 100)]
    public int enemyAmount;
    [Range(0, 100)]
    public int greenAmount;
    [Range(0, 100)]
    public int rockAmount;
    public string seed;
    public bool useRandomSeed;
    public GameObject enemy;
    public List<GameObject> greens;
    public List<GameObject> rocks;
    private int[,] map;
    private List<Coord> freeTiles;
    private int width;
    private int height;
    private int freeSpaceRadius = 3;

    public void init(int[,] mapToCopy) {
        if (useRandomSeed)
            seed = Time.time.ToString();
        width = mapToCopy.GetLength(0);
        height = mapToCopy.GetLength(1);
        map = new int[width, height];
        Buffer.BlockCopy(mapToCopy, 0, map, 0, mapToCopy.Length * sizeof(int));
        freeTiles = new List<Coord>();
        fillFreeTiles();
    }

    public void spawnEnemies() {
        spawnObject(enemy, enemiesNode, enemyAmount);
    }

    public void spawnGrass() {
        foreach (GameObject green in greens)
            spawnObject(green, worldObjectsNode, greenAmount);
    }

    public void spawnRocks() {
        foreach (GameObject rock in rocks)
            spawnObject(rock, worldObjectsNode, rockAmount);
    }

    private void spawnObject(GameObject go, GameObject parent, int amount){
        List<Coord> toRemove = new List<Coord>();
        System.Random random = new System.Random(seed.GetHashCode());
        foreach (Coord coord in freeTiles) {
            if (coord.tileX < 0 || random.Next(0, 100) > amount / 5.0f)
                continue;
            Vector3 pos = new Vector3(coord.tileX - width / 2.0f, 
                                      go.transform.position.y, 
                                      coord.tileY - height / 2.0f);
            GameObject newlyPlacedgameObject = Instantiate(go, pos, Quaternion.identity);
            newlyPlacedgameObject.transform.parent = parent.transform;
            toRemove.Add(coord);
            //Debug.Log("creating enemy " + noEnemies + " @ " + pos);
        }
        foreach (Coord coord in toRemove) {
            freeTiles.Remove(coord);
        }
    }    

    private bool freeSpace(int x, int y) {
        if (x <= freeSpaceRadius || x > width - freeSpaceRadius || 
            y <= freeSpaceRadius || y > height - freeSpaceRadius)
            return false;
        int sum = 0;
        for (int i = x - freeSpaceRadius; i < x + freeSpaceRadius; i++) {
            for (int j = y - freeSpaceRadius; j < y + freeSpaceRadius; j++) {
                //Debug.Log("@ (" + i + ", " + j + ")");
                sum += map[i, j];
            }
        }
        return sum == 0;
    }

    private struct Coord {
        public int tileX;
        public int tileY;

        public Coord(int x, int y) {
            tileX = x;
            tileY = y;
        }
    }

    public void fillFreeTiles() {
        for (int y = freeSpaceRadius; y < height; y++)
            for (int x = freeSpaceRadius; x < width; x++)
                if (freeSpace(x, y))
                    addFreeTile(x, y);
    }

    private void addFreeTile(int x, int y) {
        map[x, y] = 1;
        freeTiles.Add(new Coord(x, y));
    }
}