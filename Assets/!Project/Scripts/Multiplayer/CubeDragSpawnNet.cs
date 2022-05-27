using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CubeDragSpawnNet : NetworkBehaviour
{
    public int cubeMultiplayerPrefabSlot;
    public string triggerAxis = "XRI_Right_Trigger";
    public string toggleAxisAlignmentButton = "Fire2";
    public Transform target;

    // Server side private variables
    private GameObject cubeInstance;
    private GameObject cubePrefab;

    // Client side private variables
    private GameObject cubeProxyInstance;
    private bool hasStartedDrawing = false;

    // Shared variables (NOT SyncVars!)
    private bool axisAlignment = false;
    private Vector3 dragStartPosition;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    void EnsureInit(bool force = false)
    {
        if (!hasBeenSetup || force)
        {
            cubePrefab = NetworkManager.singleton.spawnPrefabs[cubeMultiplayerPrefabSlot];
            hasBeenSetup = true;
        }
    }

    void Awake()
    {
        EnsureInit();
    }

    // Network object state initialization
    public override void OnStartClient()
    {
        EnsureInit();
        base.OnStartClient();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority)
        { 
            if (Input.GetAxis(triggerAxis) > 0.5f && !hasStartedDrawing)
            {
                hasStartedDrawing = true;
                CmdSpawnCube(axisAlignment, target.position, target.rotation);

                // Local proxy; duplicates a lot of code...
                dragStartPosition = target.position;
                if(axisAlignment)
                {
                    cubeProxyInstance = Instantiate(cubePrefab, target.position, Quaternion.identity);
                }
                else
                {
                    cubeProxyInstance = Instantiate(cubePrefab, target.position, target.rotation);
                }
            }
            else if (Input.GetAxis(triggerAxis) > 0.5f && hasStartedDrawing)
            {
                CmdUpdateCube(); // This may be costly network-wise; should probably rely on NetworkTransform instead of calling a command every frame

                // Local proxy; duplicates a lot of code...
                if(axisAlignment)
                {
                    cubeProxyInstance.transform.localScale = target.position - dragStartPosition;
                }
                else
                {
                    cubeProxyInstance.transform.localScale = Quaternion.Inverse(cubeProxyInstance.transform.rotation)*target.position - Quaternion.Inverse(cubeProxyInstance.transform.rotation)*dragStartPosition;
                }
            }
            else
            {
                if(hasStartedDrawing)
                {
                    CmdEndCube(target.position);

                    // Local proxy no longer needed
                    Destroy(cubeProxyInstance);
                    
                    hasStartedDrawing = false;
                }
            }

            if(Input.GetButtonDown(toggleAxisAlignmentButton))
            {
                axisAlignment = !axisAlignment;
            }
        }
    }
    
    [Command]
    void CmdSpawnCube(bool alignAxis, Vector3 startPosition, Quaternion startRotation)
    {
        axisAlignment = alignAxis;
        dragStartPosition = startPosition;
        if(axisAlignment)
        {
            cubeInstance = Instantiate(cubePrefab, startPosition, Quaternion.identity);
        }
        else
        {
            cubeInstance = Instantiate(cubePrefab, startPosition, startRotation);
        }
        NetworkServer.Spawn(cubeInstance);
    }

    [Command]
    void CmdUpdateCube()
    {
        if(axisAlignment)
        {
            cubeInstance.transform.localScale = target.position - dragStartPosition;
        }
        else
        {
            cubeInstance.transform.localScale = Quaternion.Inverse(cubeInstance.transform.rotation)*target.position - Quaternion.Inverse(cubeInstance.transform.rotation)*dragStartPosition;
        }
    }

    [Command]
    void CmdEndCube(Vector3 endPosition)
    {
        if(axisAlignment)
        {
            cubeInstance.transform.localScale = endPosition - dragStartPosition;
        }
        else
        {
            cubeInstance.transform.localScale = Quaternion.Inverse(cubeInstance.transform.rotation)*endPosition - Quaternion.Inverse(cubeInstance.transform.rotation)*dragStartPosition;
        }
        cubeInstance = null;
    }
}
