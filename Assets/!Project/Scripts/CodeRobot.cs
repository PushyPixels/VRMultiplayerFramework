using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeRobot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            //Collider newCol = Instantiate(col,col.transform.position,col.transform.rotation,transform) as Collider;
            //newCol.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        }
    }
}
