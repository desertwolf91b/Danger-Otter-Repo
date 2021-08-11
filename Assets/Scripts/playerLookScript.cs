using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLookScript : MonoBehaviour
{
    private GameObject cam;
    private GameObject body;
    private float xRot;
    void Awake() { cam = GameObject.Find("FirstPersonCam"); body = GameObject.Find("Player"); Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = -Input.GetAxis("Mouse Y");

        xRot = cam.transform.localRotation.eulerAngles.x + vertical;

        if ((xRot > 80f) && (xRot < 110f))
            xRot = 80f;
        if ((xRot < 280f) && (xRot > 240f))
            xRot = 280f;

        cam.transform.localRotation = Quaternion.Euler(xRot, cam.transform.localRotation.eulerAngles.y, 0f);
        body.transform.rotation = Quaternion.Euler(0f, body.transform.rotation.eulerAngles.y + horizontal, 0f);
    }
}
