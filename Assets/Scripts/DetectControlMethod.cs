using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectControlMethod : MonoBehaviour {

    public InputController player;

	void Start () {
		
	}

    void Update() {
        if (Input.GetMouseButton(0) || 
            Input.GetMouseButton(1) || 
            Input.GetMouseButton(2) ||
            Input.GetAxisRaw("Mouse X") != 0.0 || 
            Input.GetAxisRaw("Mouse Y") != 0.0) {
            player.useController = false;
        }
        if(Input.GetAxisRaw("RHorizontal") != 0.0 || 
           Input.GetAxisRaw("RVertical") != 0.0 ||
           Input.GetKey(KeyCode.JoystickButton0) ||
           Input.GetKey(KeyCode.JoystickButton1) ||
           Input.GetKey(KeyCode.JoystickButton2) ||
           Input.GetKey(KeyCode.JoystickButton3) ||
           Input.GetKey(KeyCode.JoystickButton4) ||
           Input.GetKey(KeyCode.JoystickButton5) ||
           Input.GetKey(KeyCode.JoystickButton6) ||
           Input.GetKey(KeyCode.JoystickButton7) ||
           Input.GetKey(KeyCode.JoystickButton8) ||
           Input.GetKey(KeyCode.JoystickButton9) ||
           Input.GetKey(KeyCode.JoystickButton10)) {
            player.useController = true;
        }
    }
}
