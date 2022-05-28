using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRayNet : NetworkBehaviour
{
    public float shrinkRate = 0.5f;
    public string leftTriggerAxis = "XRI_Left_Trigger";
    public string rightTriggerAxis = "XRI_Right_Trigger";
    public string toggleGrowButton = "Fire2";

    // EnsureInit boolean
    private bool hasBeenSetup = false;
    
    private bool grow = false;

    void Update()
    {
        if(hasAuthority && LayerMask.LayerToName(gameObject.layer) == "Grabbed") // Ghetto hand check; replace with proper GrabManager stuff
        {
            if(Input.GetAxis(rightTriggerAxis) >= 0.5f)
            {
                // Raycast out and find an object to shrink/grow
                RaycastHit hit;

                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    GameObject targetRoot = hit.collider.gameObject.GetRoot();
                    if(targetRoot == null)
                    {
                        return; // No root found, abort!
                    }

                    // Check for Duplicatable; duplicate it if we find it and return
                    DuplicatableNet duplicatable = targetRoot.GetComponentInRootObject<DuplicatableNet>();
                    if(duplicatable != null)
                    {
                        duplicatable.SpawnDuplicate();
                        return; // Can't get duplicate from spawn (currently) so just fail out here and duplicate will show up eventually
                    }

                    SettableAuthority sAuth = targetRoot.GetComponentInRootObject<SettableAuthority>();
                    if(sAuth != null)
                    {
                        sAuth.SetAuthority();
                    }

                    // Actually do the shrink/grow
                    Vector3 newScale = targetRoot.transform.localScale;

                    if(grow)
                    {
                        newScale += newScale*shrinkRate*Time.deltaTime;
                    }
                    else
                    {
                        newScale -= newScale*shrinkRate*Time.deltaTime;
                    }

                    targetRoot.transform.localScale = newScale;
                }
            }
            if(Input.GetButtonDown(toggleGrowButton))
            {
                grow = !grow;
            }
        }
    }

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();
        // Remember to call all Sync functions

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