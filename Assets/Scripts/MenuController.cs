using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public bool showMenu = false;
    public bool showInv = false;
    public GameObject menu;
    public GameObject inv;
    public GameObject invItem;
    private Transform content;

    public bool paused() {
        return showMenu || showInv;
    }

    void Start () {
        content = inv.gameObject.transform.FindChild("Scroll View").
            FindChild("Viewport").FindChild("Content");
    }

    public void addToInventory(Item item) {
        GameObject gameObject = Instantiate(invItem, Vector3.zero, Quaternion.identity);
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
	
	void Update () {
		
	}

    private void setTimeScale() {
        if (paused()) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    public void inventory() {
        showMenu = false;
        menu.SetActive(showMenu);
        showInv = !showInv;
        inv.SetActive(showInv);
        setTimeScale ();
    }

    public void escape() {
        showInv = false;
        inv.SetActive(showInv);
        showMenu = !showMenu;
        menu.SetActive(showMenu);
        setTimeScale();
    }
}
