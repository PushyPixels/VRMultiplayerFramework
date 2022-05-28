using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceGoalTriggerNet : NetworkBehaviour
{
    public RaceGoalTriggerNet nextGoal;
    public TextMesh winnerDisplay;
    public GameObject objectiveArrow;
    public bool isStart = false;

    [HideInInspector]
    public bool triggerActive = false; // Doesn't have to be networked

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    // SyncVars and Local Hooks
    [SyncVar(hook = nameof(SyncWinner))]
    private string winner;
    [SyncVar(hook = nameof(SyncHasTriggered))]
    private bool hasTriggered = false;

    private void SyncWinner(string oldString, string newString)
    {
        EnsureInit();
        winner = newString;

        if(String.IsNullOrEmpty(winner))
        {
            // Reset code
            winnerDisplay.gameObject.SetActive(false);
            winnerDisplay.text = "";
        }
        else
        {
            // Player has entered the goal
            winnerDisplay.text = winner;
            winnerDisplay.gameObject.SetActive(true);
        }
    }

    private void SyncHasTriggered(bool oldBool, bool newBool)
    {
        EnsureInit();
        hasTriggered = newBool;

        if (newBool == true)
        {
            // Player has entered the goal
            if (nextGoal != null)
            {
                nextGoal.ActivateObjectiveArrow(this);
            }
        }
        else
        {
            // Reset code
            objectiveArrow.SetActive(false);
        }
    }

    // Actual reset is handled in Sync commands, this is just a catalyst (when hasTriggered is changed SyncHasTriggered fires on all clients)
    public void ResetGoal()
    {
        if(isServer)
        {
            winner = null;
            hasTriggered = false;
            triggerActive = isStart;

            RpcResetGoalOnClients(); // Fixes objective Arrow being stuck on in certain cases

            if(nextGoal != null)
            {
                nextGoal.ResetGoal();
            }
        }
    }

    [ClientRpc]
    void RpcResetGoalOnClients()
    {
        EnsureInit(true);
    }

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();
        SyncWinner(winner, winner);
        SyncHasTriggered(hasTriggered, hasTriggered);
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
            objectiveArrow.SetActive(false);
            winnerDisplay.gameObject.SetActive(false);
            winnerDisplay.text = "";

            hasBeenSetup = true;
        }
    }

    // Activate and point the objective arrow at the next goal
    public void ActivateObjectiveArrow(RaceGoalTriggerNet previousGoal)
    {
        if(nextGoal != null)
        {
            objectiveArrow.SetActive(true);
            objectiveArrow.transform.rotation = Quaternion.LookRotation(nextGoal.transform.position - transform.position,
                                                                        previousGoal.transform.position - transform.position);
        }
        else
        {
            // Display checkered flag of some kind?
        }
    }

    // Server logic
    void OnTriggerEnter(Collider other)
    {
        if(isServer && (triggerActive || isStart))
        {
            NetworkIdentity nID = other.GetComponent<NetworkIdentity>();
            if(nID != null && winner == null)
            {
                if(nextGoal == null)
                {
                    winner = nID.connectionToClient.connectionId.ToString();
                }
                else
                {
                    nextGoal.triggerActive = true;
                    hasTriggered = true;
                }
            }
        }
    }
}
