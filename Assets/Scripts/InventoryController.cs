using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    public Item head, chest, arms, legs, feet, weapon;
    public int size;
    public List<Item> items = new List<Item>();
    private MenuController menu;

    void Start() {
        menu = FindObjectOfType<MenuController>();
    }

    void Update() {

    }

    public bool canAdd() {
        return ! (items.Count >= size);
    }

    // TODO MPE: this doens't seem to be working
    public void add(Item item) {
        //Debug.Log("[inv] adding item");
        if (canAdd()) {
            //Debug.Log("[inv] success");
            items.Add(item);
            menu.addToInventory(item);
        } else {
            Debug.Log("[inv] failure");
        }
    }

    public void remove(Item item) {
        if (items.Contains(item)) {
            items.Remove(item);
            menu.removeFromInventory(item);
        }
    }
}
