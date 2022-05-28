using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    void OnTriggerEnter()//Collider other)
    {
        Destroy(this.gameObject,0.01f);
    }
}
