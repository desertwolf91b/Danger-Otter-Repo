using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLookScript : MonoBehaviour
{
    private GameObject cam;
    private GameObject body;
    void Awake() { cam = GameObject.Find("FirstPersonCam"); body = GameObject.Find("Player"); Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = -Input.GetAxis("Mouse Y");

        cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x + vertical, cam.transform.rotation.eulerAngles.y, 0f);
        body.transform.rotation = Quaternion.Euler(0f, body.transform.rotation.eulerAngles.y + horizontal, 0f);
    }
}
