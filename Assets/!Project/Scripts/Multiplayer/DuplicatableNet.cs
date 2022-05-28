using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicatableNet : NetworkBehaviour
{
    public GameObject duplicateNetworkObject;
    
    private bool hasSpawned = false;

    public void SpawnDuplicate()
    {
        if(!hasSpawned)
        {
            CmdSpawnDuplicate();
            hasSpawned = true;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnDuplicate(NetworkConnectionToClient sender = null)
    {
        if(!hasSpawned)
        {
            GameObject instance = Instantiate(duplicateNetworkObject,gameObject.transform.position,gameObject.transform.rotation);
            NetworkServer.Spawn(instance,sender);

            RpcDisableOriginal();
            gameObject.SetActive(false);
            hasSpawned = true;
        }
    }

    [ClientRpc]
    void RpcDisableOriginal()
    {
        gameObject.SetActive(false);
    }

    // EnsureInit boolean
    private bool hasBeenSetup = false;

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
            NetworkClient.RegisterPrefab(duplicateNetworkObject);
            hasBeenSetup = true;
        }
    }

}
