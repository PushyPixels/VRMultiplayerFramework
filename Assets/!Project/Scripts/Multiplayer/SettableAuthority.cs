using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettableAuthority : NetworkBehaviour
{
    public void SetAuthority()
    {
        if(isClient)
        {
            if(!hasAuthority)
            {
                CmdSetAuthority();
            }
        }
    }

    public void RemoveAuthority()
    {
        if(isServer)
        {
            NetworkIdentity identity = GetComponent<NetworkIdentity>();

            identity.RemoveClientAuthority();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSetAuthority(NetworkConnectionToClient sender = null)
    {
        NetworkIdentity identity = GetComponent<NetworkIdentity>();

        if(identity.connectionToClient != sender)
        {
            identity.RemoveClientAuthority();
            identity.AssignClientAuthority(sender);
        }
    }
}
