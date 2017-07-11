using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {

    public Transform text;
    public float flashTime;
    private float flashCount;
    private bool showing = false;

	void Start () {
		
	}
	
	void Update () {
        if (!showing) {
            return;
        }
	    if(flashCount > 0) {
            flashCount -= Time.deltaTime;
        } else {
            text.GetComponent<Text>().text = "";
            showing = false;
        }
	}

    public void showNotification(string str, float time) {
        // Debug.Log("showing notification: " + str);
        text.GetComponent<Text>().text = str;
        flashCount = time;
        showing = true;
    }

    public void showNotification(string str) {
        showNotification(str, flashTime);
    }
}
