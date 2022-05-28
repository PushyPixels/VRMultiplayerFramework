using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGravityOnButton : MonoBehaviour
{
    public string buttonName = "Fire1";
    public new Rigidbody rigidbody;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(buttonName))
        {
            rigidbody.useGravity = !rigidbody.useGravity;
        }
    }

    void OnValidate()
    {
        if(!rigidbody)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }
}
