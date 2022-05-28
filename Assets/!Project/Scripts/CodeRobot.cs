using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeRobot : MonoBehaviour
{
    public Material appleMaterial;
    public Material woodMaterial;
    public Material leafMaterial;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            int i = 0;
            foreach(Transform subchild in child)
            {
                if(i == 0)
                {
                    subchild.GetComponent<Renderer>().sharedMaterial = appleMaterial;
                }
                else if(i < 3)
                {
                    subchild.GetComponent<Renderer>().sharedMaterial = leafMaterial;
                }
                else
                {
                    subchild.GetComponent<Renderer>().sharedMaterial = woodMaterial;
                }
                i++;
            }
            //Collider newCol = Instantiate(col,col.transform.position,col.transform.rotation,transform) as Collider;
            //newCol.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        }
    }
}
