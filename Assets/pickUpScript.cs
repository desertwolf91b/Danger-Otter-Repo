using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUpScript : MonoBehaviour
{
    private Camera cam; private GameObject player; private GameObject itemContainer;
    private RawImage outline; private RawImage area;
    private Vector3 previousPosition;
    public LayerMask intractable;
    void Awake() 
    {
        cam = GameObject.Find("FirstPersonCam").GetComponent<Camera>();
        player = GameObject.Find("Player");
        itemContainer = GameObject.Find("ItemHolder");
        outline = GameObject.Find("Outline").GetComponent<RawImage>(); 
        area = GameObject.Find("Area").GetComponent<RawImage>();

        area.enabled = false; 
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 5f, intractable))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * 5f, Color.green);
            area.enabled = true;

            if ((Input.GetKeyDown(KeyCode.Mouse0)) || (Input.GetKeyDown(KeyCode.Space)))
            {
                previousPosition = hit.transform.position;
                maniuplateObject(GameObject.Find(hit.transform.name));
                setUI(false);
            }
        }
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            area.enabled = false;
        }
        if (area.transform.position != new Vector3(Screen.width / 2f, Screen.height / 2f, 0f))
            centerCrosshair();
    }

    void maniuplateObject(GameObject hitObject)
    {
        this.gameObject.GetComponent<itemLookScript>().prevPos = hitObject.transform.position;
        this.gameObject.GetComponent<itemLookScript>().prevRot = hitObject.transform.rotation;
        hitObject.transform.parent = itemContainer.transform;
        //will replace the destroy with a restriction change to make rotating objects look cooler
        //its 12:45
        //I need a break from making completely new systems
        hitObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY/* | RigidbodyConstraints.FreezeRotationZ*/;
    }

    private float fraction;
    public float duration;
    public float speed;
    void centerCrosshair()
    {
        if (fraction < 1)
        {
            if (fraction <= .2)
                fraction += Time.deltaTime / duration;

            fraction += fraction * fraction * (3f - 2f * fraction) * speed;
            area.transform.position = Vector3.Lerp(area.transform.position, new Vector3(Screen.width / 2f, Screen.height / 2f, 0f), fraction);
        }
        else
        {
            area.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            fraction = 0;
        }
            outline.transform.position = area.transform.position;
    }

    void setUI(bool activated)
    {
        area.enabled = activated;
        outline.enabled = true;

        this.gameObject.GetComponent<pickUpScript>().enabled = false;
        this.gameObject.GetComponent<playerLookScript>().enabled = false;
        this.gameObject.GetComponent<playerMoveScript>().enabled = false;
        this.gameObject.GetComponent<itemLookScript>().enabled = true;
    }
}
