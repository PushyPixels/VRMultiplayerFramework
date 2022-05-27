using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerVolumeLocallyNet : NetworkBehaviour
{
    public float newVolume = 0.5f;
    public new AudioSource audio;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();

        if(hasAuthority)
        {
            audio.volume = newVolume;
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
