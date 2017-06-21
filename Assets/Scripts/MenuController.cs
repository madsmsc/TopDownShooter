using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Transform greens, enemies;
    public GameObject character, mainMenu, inventory, inventoryItem, debug;
    private Transform content, debugList;
    private ShowWindow showWindow = ShowWindow.none;

    private enum ShowWindow { none, character, mainMenu, inventory};

    public bool paused() {
        return showWindow != ShowWindow.none;
    }

    void Start() {
        content = inventory.gameObject.transform.FindChild("Scroll View").
            FindChild("Viewport").FindChild("Content");
        debugList = debug.transform.FindChild("List");
    }

    public void addToInventory(Item item) {
        GameObject gameObject = Instantiate(inventoryItem, Vector3.zero, Quaternion.identity);
        gameObject.transform.FindChild("Text").GetComponent<Text>().text = item.itemName;
        gameObject.transform.SetParent(content);
    }

    public void removeFromInventory(Item item) {
        for(int i = 0; i < content.childCount; i++) {
            Transform child = content.GetChild(i);
            string text = child.FindChild("Text").GetComponent<Text>().text;
            if (text == item.GetInstanceID().ToString()) {
                gameObject.transform.SetParent(null);
                Destroy(gameObject);
            }
        }
    }

    private float frameCount = 0;
    private float dt = 0;
    private float fps = 0;
    private float updateRate = 4;  // 4 updates per sec.

    private void updateFps() {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate) {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }

    void Update () {
        updateFps();
        debugList.FindChild("FPS").GetComponent<Text>().text = "FPS: " + ((int)fps);
        debugList.FindChild("Greens").GetComponent<Text>().text = "Greens: " + greens.childCount;
        debugList.FindChild("Enemies").GetComponent<Text>().text = "Enemies: " + enemies.childCount;
    }

    private void setTimeScale() {
        if (paused()) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    private void toggleWindows() {
        mainMenu.SetActive(false);
        inventory.SetActive(false);
        character.SetActive(false);
        if (showWindow == ShowWindow.mainMenu)
            mainMenu.SetActive(true);
        if (showWindow == ShowWindow.inventory)
            inventory.SetActive(true);
        if (showWindow == ShowWindow.character)
            character.SetActive(true);
    }

    private void toggleGeneric(ShowWindow sw) {
        if (showWindow == sw)
            showWindow = ShowWindow.none;
        else
            showWindow = sw;
        toggleWindows();
        setTimeScale();
    }

    public void toggleCharacter() {
        toggleGeneric(ShowWindow.character);
    }

    public void toggleInventory() {
        toggleGeneric(ShowWindow.inventory);
    }

    public void toggleMainMenu() {
        toggleGeneric(ShowWindow.mainMenu);
    }
}
