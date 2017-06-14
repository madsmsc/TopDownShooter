using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootController : MonoBehaviour{
    public Currency currencyPrefab;

    private Dictionary<CurrencyType, float> chances =
        new Dictionary<CurrencyType, float>() {
            {CurrencyType.identify, 40 },
            {CurrencyType.portal, 20 },
            {CurrencyType.makeGreen, 10 },
            {CurrencyType.rerollGreen, 6 },
            {CurrencyType.secondStat, 5 },
            {CurrencyType.randomRarity, 4 },
            {CurrencyType.makeBlue, 3 },
            {CurrencyType.makeYellow, 2 },
            {CurrencyType.rerollYellow, 1 }
        };

    private static float levelMultiplier(int level) {
        if (level < -5)
            return 0;
        if (level > 5)
            return 5;

        switch (level) {
            case -5: return 0.1f;
            case -4: return 0.2f;
            case -3: return 0.4f;
            case -2: return 0.6f;
            case -1: return 0.8f;
            case 0: return 1.0f;
            case 1: return 1.2f;
            case 2: return 1.5f;
            case 3: return 1.8f;
            case 4: return 2.0f;
            case 5: return 3.0f;
            default: return 0;
        }
    }

    public Currency roll(int level) {
        float random = Random.Range(0, 100);
        foreach (KeyValuePair<CurrencyType, float> c in chances) {
            //Debug.Log("[drop] " + c.Value + ": " + random + " <= " + c.Value + "*" + levelMultiplier(level));
            if (random <= c.Value * levelMultiplier(level)) {
                //Debug.Log("[drop] yes! creating currency");
                return newCurrency(c.Key);
            }
        }
        return null;
    }

    private Currency newCurrency(CurrencyType currency) {
        // random location up to one unit away in x/z 
        Vector3 pos = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        return Instantiate<Currency>(currencyPrefab, pos, Quaternion.identity);
    }
}


/* TODO
Should I make legendaries (red) ?

remove all the mono extensions on classes where it isn't needed.
  check what it actually does. is it just for 
  start / update, or also to be able to add to a gameObject?

make a poolController that can instantiate all objects for me
  and it holds lists of objects that it recycles.
  it will have methods for instantiating:
  bullets, drops, enemies, map cells, etc.
*/

/* standard white mob drops:
item                     chance   upgrade
-----------------------------------------
identify item        	 40	      256
town portal	     	     20	      128
make item green	     	 10	      64
reroll green	     	 6	      32
add second stat	     	 5	      16	
make item random rarity	 4	      8
make item blue	     	 3	      4
make item yellow	     2	      2
reroll yellow		     1	      1 */


/* roll bonus pr mob level compared to you:
level diff     	multiplier
----------------------------------
-5			    0.1
-4			    0.2
-3			    0.4
-2			    0.6
-1			    0.8
0			    1.0
1			    1.2
2			    1.5
3			    1.8
4			    2.0
5			    3.0 */


/* item rarity:
type			affixes
-------------------------------
white			0
green			1
blue			2
yellow			3 */