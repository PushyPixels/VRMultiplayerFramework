using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastIndicator : MonoBehaviour
{
    public Transform origin;

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(origin.position,origin.forward,out hit))
        {
            transform.position = hit.point;
        }
    }
}
