using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    //public override void OnServerDisconnect(NetworkConnection conn)
    //{
        // TEMP solution; this will cause server to take authority of all objects when any client disconnects
        // What we need to do is only take authority over objects that the particular client owner
        //PreserveObjectOnServer.PreserveObjects();
        //base.OnServerDisconnect(conn);
    //}
}
