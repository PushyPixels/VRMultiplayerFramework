using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnchildSelf : MonoBehaviour
{
    void Awake()
    {
        transform.parent = null;
    }
}
