using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightMode : MonoBehaviour
{
    public float flightSpeed1 = 8.0f;
    public float flightSpeed2 = 32.0f;
    public bool flightMode = false;
    public Transform forwardSource;
    public Transform objectToFly;
    public GameObject[] stuffToDisableWhenFlying;
    public Rigidbody[] makeKinematicWhenFlying;
    public Collider[] disableCollisionWhenFlying;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("XRI_Right_Primary2DAxisClick"))
        {
            ToggleFlightMode();
        }

        if(flightMode)
        {
            if(Input.GetButton("XRI_Left_SecondaryButton") || Input.GetButton("XRI_Right_SecondaryButton"))
            {
                if(Input.GetButton("XRI_Left_SecondaryButton") && Input.GetButton("XRI_Right_SecondaryButton"))
                {
                    objectToFly.position += forwardSource.forward * flightSpeed2 * Time.deltaTime;
                }
                else
                {
                    objectToFly.position += forwardSource.forward * flightSpeed1 * Time.deltaTime;
                }
            }
        }
    }

    public void ToggleFlightMode()
    {
        Debug.Log("Toggling");
        flightMode = !flightMode;
        foreach(GameObject obj in stuffToDisableWhenFlying)
        {
            obj.SetActive(!flightMode);
        }
        foreach(Rigidbody rb in makeKinematicWhenFlying)
        {
            rb.isKinematic = flightMode;
        }
        foreach(Collider col in disableCollisionWhenFlying)
        {
            col.enabled = !flightMode;
        }
    }
}
