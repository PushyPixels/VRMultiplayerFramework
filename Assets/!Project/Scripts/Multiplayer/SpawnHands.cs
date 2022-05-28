using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHands : NetworkBehaviour
{
    private GameObject leftInstance;
    private GameObject rightInstance;
    // Start is called before the first frame update
    void Start()
    {
        if(hasAuthority)
        {
            CmdSpawn();
        }
    }

    [Command]
    void CmdSpawn()
    {
            leftInstance = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
            rightInstance = Instantiate(NetworkManager.singleton.spawnPrefabs[1]);
            NetworkServer.Spawn(leftInstance,connectionToClient);
            NetworkServer.Spawn(rightInstance,connectionToClient);
    }
}
