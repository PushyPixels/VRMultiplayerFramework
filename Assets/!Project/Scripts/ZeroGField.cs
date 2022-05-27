using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGField : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody)
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
            other.attachedRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.attachedRigidbody)
        {
            other.attachedRigidbody.useGravity = true;
        }
    }
}
