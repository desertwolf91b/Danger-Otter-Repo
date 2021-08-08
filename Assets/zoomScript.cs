using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zoomScript : MonoBehaviour
{
    private Camera cam;
    public float distance;
    public LayerMask interactable;
    public float fov;
    private float defaultFov = 90f;
    private float zoomedFov = 30f;
    private bool canZoom;
    private bool isZoomed;
    public bool outlineOn;
    public bool areaOn;
    private RawImage outline;
    private RawImage area;
    private bool firstFrame;
    void Awake() { cam = GameObject.Find("FirstPersonCam").GetComponent<Camera>(); fov = defaultFov; outline = GameObject.Find("Outline").GetComponent<RawImage>(); area = GameObject.Find("Area").GetComponent<RawImage>(); }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.gameObject.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, distance, interactable))
        {
            firstFrame = true;
            if (canZoom)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!isZoomed)
                    {
                        if (cam.fieldOfView != zoomedFov)
                        {
                            StopAllCoroutines();
                            fraction = 0f;
                            StartCoroutine(changeFOVLoop(fov, zoomedFov));
                        }
                        isZoomed = true;
                        outlineOn = false;
                    }
                    else
                    {
                        if (cam.fieldOfView != defaultFov)
                        {
                            StopAllCoroutines();
                            fraction = 0f;
                            StartCoroutine(changeFOVLoop(fov, defaultFov));
                        }
                        isZoomed = false;
                    }
                }
            }

            areaOn = true;
            if (isZoomed)
            {
                area.enabled = false;
                outline.enabled = false;
                areaOn = false;
                outlineOn = false;
            }
        }
        else
        {
            areaOn = false;
            outlineOn = true;

            if ((canZoom) && (firstFrame))
            {
                StopAllCoroutines();
                StartCoroutine(changeFOVLoop(fov, defaultFov));
                firstFrame = false;
                isZoomed = false;
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

            if (fov == defaultFov)
                outline.enabled = true;

            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(changeFOVLoop(defaultFOV_, destinationFOV_));
        }
    }
}
