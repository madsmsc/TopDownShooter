using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    public float camHeight;
    public float minCamHeight;
    public float maxCamHeight;
    public float zoomSpeed;
    public float moveSpeed;
    public bool useController; // TODO couldn't this be private?
    public GunController gun;
    public MenuController menuController;
    public List<Currency> loot; // TODO this shouldn't be here?

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private SkillController skillController;
    private Rigidbody rigidBody;
    private Camera mainCam;
    private InventoryController inv;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        mainCam = FindObjectOfType<Camera>();
        skillController = GetComponent<SkillController>();
        inv = GetComponent<InventoryController>();
        loot = new List<Currency>();
        // TODO what did this do again?
        //  Camera.main.GetComponent<Vignetting>().enabled = true/false;
    }

    void Update() {
        updateUseController();
        menuButton();
        if (menuController.paused()) {
            return;
        }
        skillButtons();
    }

    private void FixedUpdate() {
        setMoveFields();
        zoomCamera();
        rotateCamera();
        shootGun();

        rigidBody.velocity = moveVelocity;
        Vector3 camPos = new Vector3(rigidBody.position.x, camHeight, rigidBody.position.z - camHeight / 1.5f);
        mainCam.transform.position = camPos;
        pickupItems();
    }

    private void zoomCamera() {
        float zoomInput = 0;
        if (useController) {
            // TODO make camera zoom on controller
        } else {
            zoomInput = Input.GetAxis("Mouse ScrollWheel");
        }

        if (zoomInput == 0)
            return;
        camHeight -= zoomInput * zoomSpeed;
        if (camHeight < minCamHeight)
            camHeight = minCamHeight;
        if (camHeight > maxCamHeight)
            camHeight = maxCamHeight;
        //Debug.Log("camHeight=" + camHeight);
    }

    private void setMoveFields() {
        // raw is 0/1, non-raw is interpolated between 0 and 1
        // TODO should I be using raw or not? test both again...
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // normalize so moving horizontally will not move faster than along the axes
        moveInput = moveInput.normalized;
        // mult by movespeed to scale the movement
        moveVelocity = moveInput * moveSpeed;
    }

    private void menuButton() {
        if (Input.GetButtonDown("Keyboard_esc"))
            menuController.toggleMainMenu();
        else if(Input.GetButtonDown("Keyboard_i"))
            menuController.toggleInventory();
        else if (Input.GetButtonDown("Keyboard_c"))
            menuController.toggleCharacter();
        else if (Input.GetButtonDown("Keyboard_t"))
            menuController.toggleTalents();
    }

    private void skillButtons() {
        if (useController) {
            // TODO controller skill buttons?
        } else {
            for (int i = 1; i <= 6; i++) {
                if (Input.GetButtonUp("Keyboard_" + i)) {
                    skillController.useSkill(i);
                }
            }
            if (Input.GetButtonUp("Keyboard_q")) {
                gun.switchWeapon();
            }
        }
    }

    private void shootGun() {
        if (Input.GetMouseButtonDown(0) ||
                (useController && Input.GetKeyDown(KeyCode.Joystick1Button5))){
            gun.mainAttackDown();
        }
        if (Input.GetMouseButtonUp(0) ||
                (useController && Input.GetKeyUp(KeyCode.Joystick1Button5))){
            gun.mainAttackUp();
        }
        if (Input.GetMouseButtonDown(1) ||
               (useController && Input.GetKeyDown(KeyCode.Joystick1Button6))){
            gun.secondaryAttackDown();
        }
        if (Input.GetMouseButtonUp(1) ||
               (useController && Input.GetKeyUp(KeyCode.Joystick1Button6))){
            gun.secondaryAttackUp();
        }
    }

    private void rotateCamera() {
        if (useController) {
            Vector3 playerDir = Vector3.right * Input.GetAxis("RHorizontal") -
                        Vector3.forward * Input.GetAxis("RVertical");
            if (playerDir.sqrMagnitude > 0.0) {
                transform.rotation = Quaternion.LookRotation(playerDir, Vector3.up);
            }
        } else {
            Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLenth;
            if (groundPlane.Raycast(camRay, out rayLenth)) {
                Vector3 point2look = camRay.GetPoint(rayLenth);
                Debug.DrawLine(camRay.origin, point2look, Color.blue);
                Vector3 pointLevel = new Vector3(point2look.x, transform.position.y, point2look.z);
                transform.LookAt(pointLevel);
            }
        }
    }

    private void pickupItems() {
        Currency closest = null;
        float minDist = 999;
        foreach (Currency c in loot) {
            float dist = Vector3.Distance(c.transform.position, transform.position);
            if (dist < minDist) {
                closest = c;
                minDist = dist;
            }
        }

        float maxPickupDist = 2;
        if (minDist < maxPickupDist) {
            //Debug.Log("[pickup] picking up item " + closest.type);
            loot.Remove(closest);
            inv.add(closest);
            closest.gameObject.SetActive(false);
        }
    }

    private void updateUseController() {
        if (Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2) ||
            Input.GetAxisRaw("Mouse X") != 0.0 ||
            Input.GetAxisRaw("Mouse Y") != 0.0) {
            useController = false;
        }
        if (Input.GetAxisRaw("RHorizontal") != 0.0 ||
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
            useController = true;
        }
    }
}
