using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType {
    identify, portal, makeGreen, rerollGreen,
    secondStat, randomRarity, makeBlue,
    makeYellow, rerollYellow
}

public class Currency : Item {
    public CurrencyType type;
    public int amount;

    public void init() {

    }

    void OnGUI() {
        string text = type.ToString();
        if (amount > 1)
            text += " (" + amount + ")";
        // draw text as billboard with background
    }

    // TODO MPE: se video om hvordan man samler det op.
}