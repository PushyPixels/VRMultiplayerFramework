using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChildrenOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform child in transform)
        {
            child.parent = null;
        }
    }
}
