using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveObjectOnServer : NetworkBehaviour
{
    private static List<PreserveObjectOnServer> objectList = new List<PreserveObjectOnServer>();

    private void Awake()
    {
        objectList.Add(this);
    }
    public static void PreserveObjects()
    {
        foreach(PreserveObjectOnServer obj in objectList)
        {
            obj.RemoveAuthority();
        }
    }

    public void RemoveAuthority()
    {
        if(gameObject != null)
        {
            NetworkIdentity identity = GetComponent<NetworkIdentity>();
            if(identity != null)
            {
                identity.RemoveClientAuthority();
            }
        }
    }
}
