using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomScript : MonoBehaviour
{
    private Camera cam;
    public float distance;
    public LayerMask interactable;
    public float fov;
    private float defaultFov = 90f;
    private float zoomedFov = 30f;
    private bool firstFrame;
    private bool canZoom;
    void Awake() { cam = GameObject.Find("FirstPersonCam").GetComponent<Camera>(); fov = defaultFov; }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.gameObject.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, distance, interactable))
        {
            if (canZoom)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    firstFrame = true;
                    if (cam.fieldOfView != zoomedFov)
                    {
                        StopAllCoroutines();
                        StartCoroutine(changeFOVLoop(fov, zoomedFov));
                    }
                }
            }
        }
        else
        {
            if (canZoom)
            {
                if ((cam.fieldOfView != defaultFov) && (firstFrame))
                {
                    StopAllCoroutines();
                    StartCoroutine(changeFOVLoop(fov, defaultFov));
                    firstFrame = false;
                }
            }
        }

        if (canZoom)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (cam.fieldOfView != defaultFov)
                {
                    StopAllCoroutines();
                    StartCoroutine(changeFOVLoop(fov, defaultFov));
                }
            }
        }

        if ((fov == defaultFov) || (fov == zoomedFov))
            canZoom = true;
        else
            canZoom = false;

        cam.fieldOfView = fov;
    }

    public float fraction;
    public float speed;
    public float scale;
    IEnumerator changeFOVLoop(float defaultFOV_, float destinationFOV_)
    {
        yield return new WaitForSeconds(1 / speed);
        fraction += Time.deltaTime;
        fraction += fraction * fraction * (3f - 2f * fraction) / scale;
        fov = Mathf.Lerp(defaultFOV_, destinationFOV_, fraction);

        if (fraction > 1)
        {
            fov = destinationFOV_;
            fraction = 0f;
            StopAllCoroutines();
            if (!Input.GetKey(KeyCode.Mouse0) && fov != defaultFov)
                StartCoroutine(changeFOVLoop(fov, defaultFov));
        }
        else
        {
            StartCoroutine(changeFOVLoop(defaultFOV_, destinationFOV_));
        }
    }
}
