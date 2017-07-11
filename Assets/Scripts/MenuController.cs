using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public Transform worldObjectsNode, enemiesNode;
    public GameObject inventoryItem;

    private GameObject debugGO;
    private GameObject veilGO;
    private GameObject generalGO;

    private GameObject mainMenuButton;
    private GameObject mainMenuGO;

    private GameObject characterButton;
    private GameObject characterGO;

    private GameObject inventoryButton;
    private GameObject inventoryGO;

    private GameObject talentsButton;
    private GameObject talentsGO;

    private GameObject helpButton;
    private GameObject helpGO;

    private GameObject graphicsButton;
    private GameObject graphicsGO;

    private GameObject audioButton;
    private GameObject audioGO;

    private GameObject controlsButton;
    private GameObject controlsGO;

    private GameObject quitButton;
    private GameObject quitGO;

    private Transform content;
    private Transform debugList;
    private Text fpsText;
    private Text greensText;
    private Text enemiesText;

    private enum ShowWindow { none, character, inventory, talents, help, graphics, controls, audio, quit };
    private ShowWindow showWindow = ShowWindow.none;

    private Color black = new Color(0, 0, 0, 1f);
    private Color white = new Color(90.0f / 255.0f, 90.0f / 255.0f, 90.0f / 255.0f, 1f);

    private float timeSinceLastUpdate = 0;
    private float deltaTime = 0;

    void Start() {
        findChildren();
    }

    private void findChildren() {
        Transform mainMenu = transform.FindChild("Main Menu");
        mainMenuGO = mainMenu.gameObject;
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

        Transform mainMenuButtons = mainMenu.FindChild("Buttons");
        helpButton = mainMenuButtons.FindChild("Help Button").gameObject;
        graphicsButton = mainMenuButtons.FindChild("Graphics Button").gameObject;
        audioButton = mainMenuButtons.FindChild("Audio Button").gameObject;
        controlsButton = mainMenuButtons.FindChild("Controls Button").gameObject;
        quitButton = mainMenuButtons.FindChild("Quit Button").gameObject;

        helpGO = mainMenu.FindChild("Help").gameObject;
        graphicsGO = mainMenu.FindChild("Graphics").gameObject;
        audioGO = mainMenu.FindChild("Audio").gameObject;
        controlsGO = mainMenu.FindChild("Controls").gameObject;
        quitGO = mainMenu.FindChild("Quit").gameObject;

        content = inventoryGO.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content");
        debugList = debugGO.transform.FindChild("List");

        fpsText = debugList.FindChild("FPS").GetComponent<Text>();
        greensText = debugList.FindChild("Greens").GetComponent<Text>();
        enemiesText = debugList.FindChild("Enemies").GetComponent<Text>();
    }

    void Update() {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate < 0.3)
            return;
        timeSinceLastUpdate = 0;

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        int fps = (int) (1.0f / deltaTime);
        fpsText.text = "FPS: " + fps.ToString();
        greensText.text = "Greens: " + worldObjectsNode.childCount;
        enemiesText.text = "Enemies: " + enemiesNode.childCount;
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

        helpGO.SetActive(false);
        graphicsGO.SetActive(false);
        audioGO.SetActive(false);
        controlsGO.SetActive(false);
        quitGO.SetActive(false);

        mainMenuButton.GetComponent<Image>().color = black;
        inventoryButton.GetComponent<Image>().color = black;
        characterButton.GetComponent<Image>().color = black;
        talentsButton.GetComponent<Image>().color = black;

        helpButton.GetComponent<Image>().color = black;
        graphicsButton.GetComponent<Image>().color = black;
        audioButton.GetComponent<Image>().color = black;
        controlsButton.GetComponent<Image>().color = black;
        quitButton.GetComponent<Image>().color = black;
    }

    private void setWindowsActive() {
        if (showWindow == ShowWindow.help) {
            mainMenuGO.SetActive(true);
            mainMenuButton.GetComponent<Image>().color = white;
            helpGO.SetActive(true);
            helpButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.graphics) {
            mainMenuGO.SetActive(true);
            mainMenuButton.GetComponent<Image>().color = white;
            graphicsGO.SetActive(true);
            graphicsButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.audio) {
            mainMenuGO.SetActive(true);
            mainMenuButton.GetComponent<Image>().color = white;
            audioGO.SetActive(true);
            audioButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.controls) {
            mainMenuGO.SetActive(true);
            mainMenuButton.GetComponent<Image>().color = white;
            controlsGO.SetActive(true);
            controlsButton.GetComponent<Image>().color = white;
        }
        if (showWindow == ShowWindow.quit) {
            mainMenuGO.SetActive(true);
            mainMenuButton.GetComponent<Image>().color = white;
            quitGO.SetActive(true);
            quitButton.GetComponent<Image>().color = white;
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

    public void toggleHelp() {
        toggleGeneric(ShowWindow.help);
    }

    public void toggleGraphics() {
        toggleGeneric(ShowWindow.graphics);
    }

    public void toggleAudio() {
        toggleGeneric(ShowWindow.audio);
    }

    public void toggleControls() {
        toggleGeneric(ShowWindow.controls);
    }

    public void toggleQuit() {
        toggleGeneric(ShowWindow.quit);
    }

    public void reallyQuit() {
        Application.Quit();
    }
}
