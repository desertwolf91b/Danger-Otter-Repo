using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crosshairManipulation : MonoBehaviour
{
    private bool crosshairActive;
    private bool crosshairInactive;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InteractableTrigger")
        {
            crosshairActive = true;
            crosshairInactive = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractableTrigger")
        {
            crosshairInactive = true;
            crosshairActive = false;
        }
    }

    public RawImage outline;
    public RawImage area;
    private float speed = .1f;
    private float fraction;
    private float opacity_;
    void Update()
    {
        if (crosshairActive)
        {
            if (fraction < 1)
            {
                fraction += Time.deltaTime;
                fraction += fraction * fraction * (3f - 2f * fraction) * speed;
                opacity_ = Mathf.Lerp(0, 1, fraction);
            }
            else
            {
                fraction = 0;
                opacity_ = 1f;
                crosshairActive = false;
            }
        }

        if (crosshairInactive)
        {
            if (fraction < 1)
            {
                fraction += Time.deltaTime;
                fraction += fraction * fraction * (3f - 2f * fraction) * speed;
                opacity_ = 1 - Mathf.Lerp(0, 1, fraction);
            }
            else
            {
                fraction = 0;
                opacity_ = 0f;
                crosshairInactive = false;
            }
        }

        outline.color = new Color(255f, 255f, 255f, opacity_);
        area.color = new Color(255f, 255f, 255f, opacity_);
    }
}
