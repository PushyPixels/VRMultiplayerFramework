using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkJointDestruction : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncHasBeenDestroyed))]
    private bool hasBeenDestroyed = false;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    void EnsureInit(bool force = false)
    {
        if (!hasBeenSetup || force)
        {
            hasBeenSetup = true;
        }
    }

    void Awake()
    {
        EnsureInit();
    }

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();
        SyncHasBeenDestroyed(hasBeenDestroyed, hasBeenDestroyed);
        base.OnStartClient();
    }

    // Syncvar functions
    private void SyncHasBeenDestroyed(bool oldBool, bool newBool)
    {
        EnsureInit();
        hasBeenDestroyed = newBool;

        if (newBool == true)
        {
            Joint joint = GetComponent<Joint>();
            if (joint)
            {
                Destroy(joint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            if (!hasBeenDestroyed)
            {
                Joint joint = GetComponent<Joint>();
                if (!joint)
                {
                    CmdDestroyJoint();

                    hasBeenDestroyed = true; // Set value locally so that we don't send more commands
                }
            }
        }
    }

    [Command(requiresAuthority = false)] // May not need ignoreAuthority but don't want to test today (2/21/21)
    private void CmdDestroyJoint()
    {
        hasBeenDestroyed = true;
    }
}
