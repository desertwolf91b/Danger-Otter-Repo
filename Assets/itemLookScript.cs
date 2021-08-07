using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemLookScript : MonoBehaviour
{
    /*
     * for real, good luck understanding all this
     * its straight up spaghetti code
     * at least it gets the job done and is repeatable
     */

    //brick of objects and stuff
    private GameObject item; private Rigidbody rb;
    private bool activated; private bool isTurning;
    private RawImage outline; private RawImage area;
    public float sensitivity; public float force;
    public Vector3 prevPos; public Quaternion prevRot;
    private GameObject container;
    private bool isActive = true;
    void Awake() { area = GameObject.Find("Area").GetComponent<RawImage>(); outline = GameObject.Find("Outline").GetComponent<RawImage>(); container = GameObject.Find("ItemHolder"); }
    void Update()
    {
        //determines if it is being held or not
        //then gets the rigidbody that will be used
        if (!activated)
        {
            if (this.gameObject.transform.GetChild(2).GetChild(0).gameObject != null)
            {
                item = this.gameObject.transform.GetChild(2).GetChild(0).gameObject;
                rb = item.GetComponent<Rigidbody>();
                activated = true;
                isActive = true;
            }
            //a few variables that need to be reset
            fraction = 0f;
            firstFrame = true;
            reversed = false;
        }
        else
        {
            //needs a clamping force or the cursor can super easily be lost
            if (isActive)
            {
                outline.transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime, Input.GetAxisRaw("Mouse Y") * Time.deltaTime, 0f) * sensitivity * Screen.width;
                area.transform.position = outline.transform.position;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                area.enabled = true;
                isTurning = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                area.enabled = false;
                isTurning = false;
            }

            if (isTurning)
            {
                //rb.AddTorque(new Vector3(Input.GetAxisRaw("Mouse Y"), -Input.GetAxisRaw("Mouse X"), 0f) * force, ForceMode.Acceleration);
                rb.AddTorque(transform.right * Input.GetAxisRaw("Mouse Y") * force + transform.up * -Input.GetAxisRaw("Mouse X") * force, ForceMode.Acceleration);
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                isActive = false;
                complete = false;
                item.transform.parent = null;

                //ugly code but this just reverts everything back to before this script was activated
                GetComponent<playerMoveScript>().enabled = true;
                GetComponent<playerLookScript>().enabled = true;
                GetComponent<pickUpScript>().enabled = true;
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                reversed = true;
            }
        }
        handleLerp();
    }


    //I really don't wanna explain all of this
    //I have no idea how this is working so well
    //don't question it
    private bool reversed;
    private float fraction;
    private bool firstFrame;
    private Vector3 prevPosA;
    private Quaternion prevRotA;
    public float duration;
    private float time;
    public float speed;
    private bool complete;
    void handleLerp()
    {
        if (!complete)
        {
            if (!reversed)
            {
                if (firstFrame)
                {
                    prevPosA = item.transform.position;
                }
                if (fraction < 1)
                {
                    if (fraction <= .2)
                        fraction += Time.deltaTime / duration;

                    fraction += fraction * fraction * (3f - 2f * fraction) * speed;
                    item.transform.position = Vector3.Lerp(prevPosA, container.transform.position, fraction);
                }
                else
                {
                    item.transform.position = container.transform.position;
                    fraction = 0;
                    complete = true;
                }
            }
            else
            {
                if (firstFrame)
                {
                    prevPosA = item.transform.position;
                    prevRotA = item.transform.rotation;
                }
                if (fraction < 1)
                {
                    if (fraction <= .2)
                        fraction += Time.deltaTime / duration;

                    fraction += fraction * fraction * (3f - 2f * fraction) * speed;
                    item.transform.position = Vector3.Lerp(prevPosA, prevPos, fraction);
                    item.transform.rotation = Quaternion.Lerp(prevRotA, prevRot, fraction);
                }
                else
                {
                    //another set of resetting code
                    //some is probably redundant but I'll come back and optomize later
                    item.transform.position = prevPos;
                    
                    //area.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                    //outline.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

                    complete = false;

                    prevPos = new Vector3(0f, 0f, 0f);
                    prevPosA = new Vector3(0f, 0f, 0f);
                    fraction = 0;
                    reversed = false; activated = false;
                    item = null;

                    GetComponent<itemLookScript>().enabled = false;
                }
            }
        }
    }
}