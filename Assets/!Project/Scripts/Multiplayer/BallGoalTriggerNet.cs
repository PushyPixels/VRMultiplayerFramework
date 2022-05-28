using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGoalTriggerNet : NetworkBehaviour
{
    public TextMesh scoreDisplay;
    public Transform ballSpawnPosition;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    // SyncVars and Local Hooks
    [SyncVar(hook = nameof(SyncScore))]
    private int score;

    private void SyncScore(int oldValue, int newValue)
    {
        EnsureInit();
        score = newValue;

        scoreDisplay.text = score.ToString();
    }

    public void ResetScore()
    {
        if(isServer)
        {
            EnsureInit(true); // Should do everything?  If not we need to fire RPC
        }
    }


    // May not need this but keeping it around just in case for the moment
    [ClientRpc]
    void RpcResetScoreOnClients()
    {
        EnsureInit(true);
    }

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();
        SyncScore(score, score);
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
            score = 0;
            hasBeenSetup = true;
        }
    }


    // Server logic
    void OnTriggerEnter(Collider other)
    {
        if(isServer)
        {
            score++;

            Rigidbody rb = other.attachedRigidbody;
            rb.GetComponent<SettableAuthority>().RemoveAuthority();
            rb.transform.position = ballSpawnPosition.position;
        }
    }
}
