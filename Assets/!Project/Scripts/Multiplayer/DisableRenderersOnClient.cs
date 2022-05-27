using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRenderersOnClient : NetworkBehaviour
{
    public Renderer[] renderers;

    private bool hasDisabledRenderers = false;

    void Update()
    {
        if(!hasDisabledRenderers && hasAuthority)
        {
            foreach(Renderer renderer in renderers)
            {
                renderer.enabled = false;
                hasDisabledRenderers = true;
            }
        }
    }
}
