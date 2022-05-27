using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRigidbodyOnClients : NetworkBehaviour
{
    [HideInInspector]
    public bool willHaveAuthority;
    private bool timerStarted;
    private float willHaveAuthorityTimeout;

    // Start is called before the first frame update
    void Update()
    {
        if(hasAuthority || willHaveAuthority)
        {
            foreach(Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
            {
                rigidbody.isKinematic = false;
            }
            if(!timerStarted)
            {
                willHaveAuthorityTimeout = 1.0f;
                timerStarted = true;
            }
        }
        else
        {
            foreach(Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
            {
                rigidbody.isKinematic = true;
            }
        }

        if((hasAuthority && willHaveAuthority) || willHaveAuthorityTimeout <= 0.0f)
        {
            willHaveAuthority = false;
            timerStarted = false;
        }

        if(timerStarted)
        {
            willHaveAuthorityTimeout -= Time.deltaTime;
        }
    }
}
