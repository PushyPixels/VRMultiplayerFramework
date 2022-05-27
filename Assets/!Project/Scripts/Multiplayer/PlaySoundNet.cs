using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundNet : NetworkBehaviour
{
    public AudioSource audioSource;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    public bool isPlaying => audioSource.isPlaying;

    public void PlaySound()
    {
        if(hasAuthority)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play(); // Play locally
            }
            CmdPlaySound(); // Tell server to have other players play too
        }
    }

    public void StopSound()
    {
        if(hasAuthority)
        {
            audioSource.Stop(); // Stop locally
            CmdStopSound(); // Tell server to have other players stop too
        }
    }

    [Command]
    private void CmdPlaySound()
    {
        RpcPlaySound();
    }

    [Command]
    private void CmdStopSound()
    {
        RpcStopSound();
    }

    [ClientRpc(includeOwner = false)]
    private void RpcPlaySound()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    [ClientRpc(includeOwner = false)]
    private void RpcStopSound()
    {
        audioSource.Stop();
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
