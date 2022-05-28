using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTaggedObject : MonoBehaviour
{
    public string tagToFollow;
    public bool followX = true;
    public bool followY = true;
    public bool followZ = true;

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(tagToFollow).transform;
    }

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;

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
        
        transform.position = newPosition;
        transform.rotation = target.rotation;
    }
}
