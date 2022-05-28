using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public PlaySoundNet leftThruster;
    public PlaySoundNet rightThruster;

    public static NetworkPlayer instance;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();

        if(hasAuthority)
        {
            instance = this;
            LocalPlayer.instance.networkPlayer = this;
        }

        base.OnStartClient();
    }

    // Local logic
    void Awake()
    {
        EnsureInit();
    }

    // This is called everywhere a function might be called by the server before Awake()
    void EnsureInit(bool force = false)
    {
        if (!hasBeenSetup || force)
        {
            // Initialization code goes here

            hasBeenSetup = true;
        }
    }
}
