using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictiveRigidbodyAuthority : MonoBehaviour
{
    public float scaleOverride = 0.0f;
    public new Rigidbody rigidbody;

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hitInfo;

        float scale;

        if(scaleOverride != 0.0f)
        {
            scale = scaleOverride;
        }
        else
        {
            scale = transform.lossyScale.x/2.0f;
        }

        if(Physics.SphereCast(rigidbody.position,scale,rigidbody.velocity, out hitInfo, rigidbody.velocity.magnitude*1.1f))
        {
            Rigidbody rb = hitInfo.collider.attachedRigidbody;
            SettableAuthority sa = null;
            NoRigidbodyOnClients nrb = null;

            if(rb)
            {
                sa = rb.GetComponent<SettableAuthority>();
                nrb = rb.GetComponent<NoRigidbodyOnClients>();;
            }
            if(sa)
            {
                sa.SetAuthority();
            }
            if(nrb)
            {
                nrb.willHaveAuthority = true;
            }
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
