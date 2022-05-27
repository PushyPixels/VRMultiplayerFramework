using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTaggedObjectNet : NetworkBehaviour
{
    public string tagToFollow;
    public bool followX = true;
    public bool followY = true;
    public bool followZ = true;

    public Transform follower;

    private Transform target;

    void OnValidate()
    {
        if(follower == null)
        {
            follower = transform;
        }
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(tagToFollow).transform;
    }

    void LateUpdate()
    {
        if(hasAuthority)
        {
            Vector3 newPosition = follower.position;

            if(followX)
            {
                newPosition.x = target.position.x;
            }

            if(followY)
            {
                newPosition.y = target.position.y;
            }

            if(followZ)
            {
                newPosition.z = target.position.z;
            }
        
            follower.position = newPosition;
            follower.rotation = target.rotation;
        }
    }
}
