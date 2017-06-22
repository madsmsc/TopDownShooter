using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public Transform worldObjectsNode, enemiesNode;
    public GameObject inventoryItem;

    private GameObject mainMenuGO;
    private GameObject characterGO;
    private GameObject inventoryGO;
    private GameObject talentsGO;
    private GameObject debugGO;
    private GameObject veilGO;
    private GameObject generalGO;

    private GameObject mainMenuButton;
    private GameObject characterButton;
    private GameObject inventoryButton;
    private GameObject talentsButton;

    private Transform content;
    private Transform debugList;
    private ShowWindow showWindow = ShowWindow.none;

    private Color black = new Color(0, 0, 0, 0.7f);
    private Color white = new Color(1, 1, 1, 0.7f);
    private float frameCount = 0;
    private float dt = 0;
    private float fps = 0;
    private float updateRate = 4;  // 4 updates per sec.

    private enum ShowWindow { none, mainMenu, character, inventory, talents };

    void Start() {
        findChildren();
    }

    private void findChildren() {
        mainMenuGO = transform.FindChild("Main Menu").gameObject;
        characterGO = transform.FindChild("Character").gameObject;
        inventoryGO = transform.FindChild("Inventory").gameObject;
        talentsGO = transform.FindChild("Talents").gameObject;
        debugGO = transform.FindChild("Debug").gameObject;
        veilGO = transform.FindChild("Veil").gameObject;
        generalGO = transform.FindChild("General").gameObject;

        Transform menuButtons = transform.FindChild("General").FindChild("Menu Buttons");
        mainMenuButton = menuButtons.FindChild("Main Menu Button").gameObject;
        characterButton = menuButtons.FindChild("Character Button").gameObject;
        inventoryButton = menuButtons.FindChild("Inventory Button").gameObject;
        talentsButton = menuButtons.FindChild("Talents Button").gameObject;

        content = inventoryGO.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content");
        debugList = debugGO.transform.FindChild("List");
    }

    void Update() {
        updateFps();
        debugList.FindChild("FPS").GetComponent<Text>().text = "FPS: " + ((int)fps);
        debugList.FindChild("Greens").GetComponent<Text>().text = "Greens: " + worldObjectsNode.childCount;
        debugList.FindChild("Enemies").GetComponent<Text>().text = "Enemies: " + enemiesNode.childCount;
    }

    public bool paused() {
        return showWindow != ShowWindow.none;
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

    private void updateFps() {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate) {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }

    private void setTimeScale() {
        if (paused()) {
            Time.timeScale = 0;
            veilGO.SetActive(true);
            generalGO.SetActive(true);
        } else {
            Time.timeScale = 1;
            veilGO.SetActive(false);
            generalGO.SetActive(false);
        }
    }

    private void setWindowsInactive() {
        mainMenuGO.SetActive(false);
        inventoryGO.SetActive(false);
        characterGO.SetActive(false);
        talentsGO.SetActive(false);

        mainMenuButton.GetComponent<Image>().color = black;
        inventoryButton.GetComponent<Image>().color = black;
        characterButton.GetComponent<Image>().color = black;
        talentsButton.GetComponent<Image>().color = black;
    }

    private void setWindowsActive() {
        if (showWindow == ShowWindow.mainMenu) {
            mainMenuGO.SetActive(true);
            mainMenuButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.inventory) {
            inventoryGO.SetActive(true);
            inventoryButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.character) {
            characterGO.SetActive(true);
            characterButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.talents) {
            talentsGO.SetActive(true);
            talentsButton.GetComponent<Image>().color = white;
        }
    }

    private void toggleWindows() {
        setWindowsInactive();
        setWindowsActive();
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

    public void toggleTalents() {
        toggleGeneric(ShowWindow.talents);
    }

    public void toggleMainMenu() {
        toggleGeneric(ShowWindow.mainMenu);
    }
}
