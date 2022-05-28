using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManagerNet : NetworkBehaviour
{
    public Dictionary<string,AudioSource> audioDatabase = new Dictionary<string,AudioSource>();

    // EnsureInit boolean
    private bool hasBeenSetup = false;
    

    public void PlaySound(string soundName)
    {
        if(hasAuthority)
        {
            CmdPlaySound(soundName);
        }
    }

    public void StopSound(string soundName)
    {
        if(hasAuthority)
        {
            CmdStopSound(soundName);
        }
    }

    [Command]
    private void CmdPlaySound(string soundName)
    {
        RpcPlaySound(soundName);
    }

    [Command]
    private void CmdStopSound(string soundName)
    {
        RpcStopSound(soundName);
    }

    [ClientRpc]
    private void RpcPlaySound(string soundName)
    {
        audioDatabase[soundName].Play();
    }

    [ClientRpc]
    private void RpcStopSound(string soundName)
    {
        audioDatabase[soundName].Stop();
    }

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();
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
