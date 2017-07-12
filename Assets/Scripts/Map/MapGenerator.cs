using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {
    [Range(0, 100)]
    public int randomFillPercent;
    public int width;
    public int height;
    public bool useRandomSeed;
    public string seed;
    public GameObject portalPrefab;
    public Transform player;
    public NotificationController notifications;
    public GameObject sunlight;
    public bool night;

    private int[,] map;
    private bool nextLevel, previousLevel;
    private enum MapType { rockyForest, cave, floating };
    private GameObject startPortalGO, endPortalGO;
    private ObjectSpawner objectSpawner;
    private BulletPool bulletPool;
    private EnemyPool enemyPool;
    private GreenPool greenPool;
    private LootPool lootPool;

    void Start() {
        bulletPool = transform.FindChild("Bullets").GetComponent<BulletPool>();
        enemyPool = transform.FindChild("Enemies").GetComponent<EnemyPool>();
        greenPool = transform.FindChild("Greens").GetComponent<GreenPool>();
        lootPool = transform.FindChild("Loot").GetComponent<LootPool>();
        objectSpawner = GetComponent<ObjectSpawner>();
        generateMap(MapType.rockyForest);
    }

    void Update() {
        // TODO: this absolutely shouldn't be here
        float flashTime = 0.5f;
        previousLevel = Vector3.Distance(player.position, startPortalGO.transform.position) < 3;
        nextLevel = Vector3.Distance(player.position, endPortalGO.transform.position) < 3;
        if (previousLevel) {
            notifications.showNotification("Press [SPACE] to go to previous level!", flashTime);
        } else if (nextLevel) {
            notifications.showNotification("Press [SPACE] to go to next level!", flashTime);
        } 

        // updateMapOnMouse0();
    }

    public void changeLevel() {
        if (!nextLevel && !previousLevel)
            return;
        // TODO this doens't work at all... da fudge?
        //TODO  move this to objectSpawner
        List<GreenController> greenToDelete = new List<GreenController>(greenPool.active);
        foreach (GreenController green in greenToDelete) {
            Debug.Log("removing the green " + green);
            greenPool.destroyObject(green);
        }
        List<EnemyController> enemyToDelete = new List<EnemyController>(enemyPool.active);
        foreach (EnemyController enemy in enemyToDelete) {
            Debug.Log("removing the enemy " + enemy);
            enemyPool.destroyObject(enemy);
        }
        List<BulletController> bulletToDelete = new List<BulletController>(bulletPool.active);
        foreach (BulletController bullet in bulletToDelete) {
            Debug.Log("removing the bullet " + bullet);
            bulletPool.destroyObject(bullet);
        }
        List<Currency> currencyToDelete = new List<Currency>(lootPool.active);
        foreach (Currency currency in currencyToDelete) {
            lootPool.destroyObject(currency);
        }
        generateMap(MapType.rockyForest);
    }

    private void updateMapOnMouse0() {
        if (Input.GetMouseButtonDown(0)) {
            generateMap(MapType.rockyForest);
        }
    }

    private void generateMap(MapType mapType) {
        initMaps();
        randomFillMap();
        for (int i = 0; i < 5; i++) {
            smoothMap();
        }
        processMap();
        int[,] borderedMap = fillMap();
        generateObjets(mapType);
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.generateMesh(borderedMap, 1);
    }

    private void generateObjets(MapType mapType) {
        //debugMap(borderedMap);
        objectSpawner.init(map);
        objectSpawner.spawnEnemies();
        if (mapType == MapType.rockyForest) {
            objectSpawner.spawnGrass();
            objectSpawner.spawnRocks();
        } if (mapType == MapType.cave) {
            objectSpawner.spawnRocks();
        } if (mapType == MapType.floating) {
            // TODO make the cave flat, and make the meshGenerator make mesh for 0's instead of 1s.
        }
    }

    private void initMaps() {
        int maxDimension = 200;
        if (width > maxDimension)
            width = maxDimension;
        if (height > maxDimension)
            height = maxDimension;

        map = new int[width, height];
    }

    private int[,] fillMap() {
        int borderSize = 1;
        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++) {
            for (int y = 0; y < borderedMap.GetLength(1); y++) {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                } else {
                    borderedMap[x, y] = 1;
                }
            }
        }
        return borderedMap;
    }

    private void debugMap(int[,] borderedMap) {
        Debug.Log("bordered map:");
        String mapString = "";
        for (int y = 0; y < borderedMap.GetLength(1); y++) {
            for (int x = 0; x < borderedMap.GetLength(0); x++) {
                mapString += borderedMap[x, y] + ", ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }

    private void processWallRegions() {
        List<List<Coord>> wallRegions = getRegions(1);
        int wallThresholdSize = 50;

        foreach (List<Coord> wallRegion in wallRegions) {
            if (wallRegion.Count < wallThresholdSize) {
                foreach (Coord tile in wallRegion) {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }
    }

    private Vector3 makeStartPortal(int offset) {
        Vector3 startPortalPos = new Vector3(-width / 2.0f + offset, 1, -height / 2.0f + offset);
        startPortalGO = Instantiate(portalPrefab, startPortalPos, Quaternion.identity);
        startPortalGO.transform.parent = greenPool.transform;
        return startPortalPos;
    }

    private Vector3 makeEndPortal(int offset) {
        Vector3 endPortalPos = new Vector3(width / 2.0f - offset, 1, height / 2.0f - offset);
        endPortalGO = Instantiate(portalPrefab, endPortalPos, Quaternion.identity);
        endPortalGO.transform.parent = greenPool.transform;
        return endPortalPos;
    }

    private void makePortalLights(Vector3 startPos, Vector3 endPos) {
        // TODO: make portal GOs have position and cubes be at (0,0,0)
        // that way, the lights will just inherit the position when their parents are the portals

        Color lightColor = new Color(1, 1, 1, 1);
        GameObject startLightGO = new GameObject("Start Portal Light");
        Light startLightComp = startLightGO.AddComponent<Light>();
        startLightComp.color = lightColor;
        startLightGO.transform.parent = startPortalGO.transform;
        startLightGO.transform.position = startPos;
        GameObject endLightGO = new GameObject("End Portal Light");
        Light endLightComp = endLightGO.AddComponent<Light>();
        endLightComp.color = lightColor;
        endLightGO.transform.parent = endPortalGO.transform;
        endLightGO.transform.position = endPos;
    }

    private void makeNightDay(Vector3 startPos, Vector3 endPos) {
        if (night) {
            // reduce ambient light and disable sunlight
            RenderSettings.ambientIntensity = 0.1f;
            sunlight.SetActive(false);
            makePortalLights(startPos, endPos);
        } else {
            RenderSettings.ambientIntensity = 0.66f;
            sunlight.SetActive(true);
        }
    }

    private void makeStartEndRooms(List<Room> survivingRooms) {
        int offset = 5;
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        Vector3 startPos = makeStartPortal(offset);
        Vector3 endPos = makeEndPortal(offset);
        makeNightDay(startPos, endPos);
        player.position = new Vector3(startPos.x, player.position.y, startPos.z);

        Room startRoom = new Room(new List<Coord>() {
            new Coord(offset, offset), new Coord(offset+1, offset+1),
            new Coord(offset+1, offset), new Coord(offset, offset+1)}, map);
        Room endRoom = new Room(new List<Coord>() {
            new Coord(width-offset, height-offset), new Coord(width-offset-1, height-offset-1),
            new Coord(width-offset, height-offset-1), new Coord(width-offset-1, height-offset)}, map);
        survivingRooms.Add(startRoom);
        survivingRooms.Add(endRoom);
    }

    private List<Room> processRoomRegions() {
        List<List<Coord>> roomRegions = getRegions(0);
        List<Room> survivingRooms = new List<Room>();
        int roomThresholdSize = 50;
        makeStartEndRooms(survivingRooms);

        foreach (List<Coord> roomRegion in roomRegions) {
            if (roomRegion.Count < roomThresholdSize) {
                foreach (Coord tile in roomRegion) {
                    map[tile.tileX, tile.tileY] = 1;
                }
            } else {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }
        return survivingRooms;
    }

    private void processMap() {
        processWallRegions();
        List<Room> survivingRooms = processRoomRegions();
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        connectClosestRooms(survivingRooms);
    }

    private void connectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom) {
            foreach (Room room in allRooms) {
                if (room.isAccessibleFromMainRoom) {
                    roomListB.Add(room);
                } else {
                    roomListA.Add(room);
                }
            }
        } else {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA) {
            if (!forceAccessibilityFromMainRoom) {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0) {
                    continue;
                }
            }

            foreach (Room roomB in roomListB) {
                if (roomA == roomB || roomA.isConnected(roomB)) {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
                createPassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
            createPassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            connectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom) {
            connectClosestRooms(allRooms, true);
        }
    }

    private void createPassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
        Room.connectRooms(roomA, roomB);
        Debug.DrawLine(coordToWorldPoint(tileA), coordToWorldPoint(tileB), Color.green, 100);

        List<Coord> line = getLine(tileA, tileB);
        foreach (Coord c in line) {
            drawCircle(c, 5);
        }
    }

    private void drawCircle(Coord c, int r) {
        for (int x = -r; x <= r; x++) {
            for (int y = -r; y <= r; y++) {
                if (x * x + y * y <= r * r) {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if (isInMapRange(drawX, drawY)) {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    private List<Coord> getLine(Coord from, Coord to) {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest) {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++) {
            line.Add(new Coord(x, y));

            if (inverted) {
                y += step;
            } else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                } else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    private Vector3 coordToWorldPoint(Coord tile) {
        return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
    }

    private List<List<Coord>> getRegions(int tileType) {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
                    List<Coord> newRegion = getRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion) {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    private List<Coord> getRegionTiles(int startX, int startY) {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0) {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                    if (isInMapRange(x, y) && (y == tile.tileY || x == tile.tileX)) {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    private bool isInMapRange(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }


    private void randomFillMap() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
                    map[x, y] = 1;
                } else {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    private void smoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbourWallTiles = getSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    private int getSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (isInMapRange(neighbourX, neighbourY)) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX, neighbourY];
                    }
                } else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    private struct Coord {
        public int tileX;
        public int tileY;

        public Coord(int x, int y) {
            tileX = x;
            tileY = y;
        }
    }

    private class Room : IComparable<Room> {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room() {
        }

        public Room(List<Coord> roomTiles, int[,] map) {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles) {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                        if (x == tile.tileX || y == tile.tileY) {
                            if (map[x, y] == 1) {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void setAccessibleFromMainRoom() {
            if (!isAccessibleFromMainRoom) {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms) {
                    connectedRoom.setAccessibleFromMainRoom();
                }
            }
        }

        public static void connectRooms(Room roomA, Room roomB) {
            if (roomA.isAccessibleFromMainRoom) {
                roomB.setAccessibleFromMainRoom();
            } else if (roomB.isAccessibleFromMainRoom) {
                roomA.setAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool isConnected(Room otherRoom) {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom) {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}