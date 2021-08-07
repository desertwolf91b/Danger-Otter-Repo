using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoveScript : MonoBehaviour
{
    private CharacterController controller;
    public float gravityScale;
    private float y; public float Speed;
    private float x; private float z;
    void Awake() { controller = GetComponent<CharacterController>(); }
    void Update()
    {
        //simple application of linear gravity
        //no variable of drag or accelaration (yet)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.white);
            y = hit.distance;
        }

        //adjust WASD movements to make it more sluggish
        //still linear but will fine tune later
        switch (Input.GetAxisRaw("Vertical"))
        {
            case 1:
                {
                    x += .5f * Time.deltaTime;
                    x = Mathf.Clamp(x, -1, 1);
                    break;
                }
            case -1:
                {
                    x += -.5f * Time.deltaTime;
                    x = Mathf.Clamp(x, -1, 1);
                    break;
                }
            case 0:
                {
                    if (x < 0)
                        x += 1f * Time.deltaTime;
                    if (x > 0)
                        x -= 1f * Time.deltaTime;
                    break;
                }
        }

        switch (Input.GetAxisRaw("Horizontal"))
        {
            case 1:
                {
                    z += .5f * Time.deltaTime;
                    z = Mathf.Clamp(z, -1, 1);
                    break;
                }
            case -1:
                {
                    z += -.5f * Time.deltaTime;
                    z = Mathf.Clamp(z, -1, 1);
                    break;
                }
            case 0:
                {
                    if (z < 0)
                        z += 1f * Time.deltaTime;
                    if (z > 0)
                        z -= 1f * Time.deltaTime;
                    break;
                }
        }

        //applies adjustments
        float horizontal = z * Speed * Time.deltaTime;
        float vertical = x * Speed * Time.deltaTime;

        //applies movement to playercontroller
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        //simple gravity function
        if (y >= 1.09f)
            controller.Move(transform.up * -gravityScale * Time.deltaTime);
        else
            controller.Move(move);
    }
}
