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
            Vector3 newPosition = Vector3.zero;
            Quaternion newRotation = Quaternion.identity;

            List<Transform> childList = new List<Transform>();
            foreach(Transform subchild in child)
            {
                childList.Add(subchild);
                if(i == 0)
                {
                    //subchild.GetComponent<Renderer>().sharedMaterial = appleMaterial;
                    //Rigidbody rb = subchild.gameObject.AddComponent<Rigidbody>();
                    //if(rb != null)
                    //{
                    //}

                    newPosition = subchild.position;
                    newRotation = subchild.rotation;
                }
                else if(i < 3)
                {
                    //subchild.GetComponent<Renderer>().sharedMaterial = leafMaterial;
                }
                else
                {
                    //subchild.GetComponent<Renderer>().sharedMaterial = woodMaterial;
                }



                i++;
            }


            foreach(Transform oldChild in childList)
            {
                oldChild.parent = null;
            }

            child.position = newPosition;
            child.rotation = newRotation;

            foreach(Transform oldChild in childList)
            {
                oldChild.parent = child;
            }
            //Collider newCol = Instantiate(col,col.transform.position,col.transform.rotation,transform) as Collider;
            //newCol.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        }
    }
}
