using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroHorizontalVelocity : MonoBehaviour
{
    public new Rigidbody rigidbody;

    void OnValidate()
    {
        if(!rigidbody)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector3.Scale(rigidbody.velocity,Vector3.up);
    }
}
