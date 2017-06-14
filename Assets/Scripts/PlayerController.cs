using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float camHeight, minCamHeight, maxCamHeight, zoomSpeed;
    public float moveSpeed;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    public bool useController;
    public GunController gun;
    private SkillController skillController;
    private Rigidbody rigidBody;
    private Camera mainCam;
    private MenuController menuController;
    private InventoryController inv;
    public List<Currency> loot;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        mainCam = FindObjectOfType<Camera>();
        menuController = GetComponent<MenuController>();
        skillController = GetComponent<SkillController>();
        inv = GetComponent<InventoryController>();
        loot = new List<Currency>();
        //  Camera.main.GetComponent<Vignetting>().enabled = true/false;
    }

    void Update() {
        menuButton();
        if (menuController.paused()) {
            return;
        }
        setMoveFields();
        if (!useController) {
            rotateWithMouse();
            skillButtons();
            shootGun();
            zoomCamera();
        } else {
            rotateWithController();
        }
	}

    private void zoomCamera() {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
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
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // normalize so moving horizontally will not move faster than along the axes
        moveInput = moveInput.normalized;
        // mult by movespeed to scale the movement
        moveVelocity = moveInput * moveSpeed;
    }

    private void rotateWithMouse() {
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

    private void menuButton() {
        if (Input.GetButtonDown("Keyboard_esc")) {
            menuController.escape();
        } else if(Input.GetButtonDown("Keyboard_i")) {
            menuController.inventory();
        }
    }

    private void skillButtons() {
        for(int i = 1; i <= 6; i++) {
            if (Input.GetButtonDown("Keyboard_"+i)) {
                skillController.useSkill(i);
            }
        }
        if (Input.GetButtonDown("Keyboard_q")) {
            gun.switchWeapon();
        }
    }

    private void shootGun() {
        // mouse buttons: 0 left, 1 right, 2 middle
        if (Input.GetMouseButtonDown(0)) {
            gun.mainAttackDown();
        }
        if (Input.GetMouseButtonUp(0)) {
            gun.mainAttackUp();
        }
        if (Input.GetMouseButtonDown(1)) {
            gun.secondaryAttackDown();
        }
        if (Input.GetMouseButtonUp(1)) {
            gun.secondaryAttackUp();
        }
    }

    private void rotateWithController() {
        Vector3 playerDir = Vector3.right * Input.GetAxis("RHorizontal") -
                    Vector3.forward * Input.GetAxis("RVertical");
        if (playerDir.sqrMagnitude > 0.0) {
            transform.rotation = Quaternion.LookRotation(playerDir, Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button5)) {
            gun.isFiring = true;
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button5)) {
            gun.isFiring = false;
        }
    }

    private void FixedUpdate () {
        rigidBody.velocity = moveVelocity;
        Vector3 camPos = new Vector3(rigidBody.position.x, camHeight, rigidBody.position.z - camHeight/1.5f);
        mainCam.transform.position = camPos;
        pickupItems();
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
            Debug.Log("[pickup] picking up item " + closest.type);
            loot.Remove(closest);
            inv.add(closest);
            closest.gameObject.SetActive(false);
        }
    }
}
