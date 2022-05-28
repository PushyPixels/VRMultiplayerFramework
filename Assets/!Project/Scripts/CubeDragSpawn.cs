using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDragSpawn : MonoBehaviour
{
    public Rigidbody cubePrefab;
    public string triggerAxis = "XRI_Right_Trigger";
    public string toggleAxisAlignmentButton = "Fire2";

    private Vector3 dragStartPosition;
    private bool hasStartedDrawing = false;
    private Rigidbody cubeInstance;
    private bool axisAlignment = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(triggerAxis) > 0.5f && !hasStartedDrawing)
        {
            hasStartedDrawing = true;
            dragStartPosition = transform.position;
            if(axisAlignment)
            {
                cubeInstance = Instantiate(cubePrefab, transform.position, Quaternion.identity) as Rigidbody;
            }
            else
            {
                cubeInstance = Instantiate(cubePrefab, transform.position, transform.rotation) as Rigidbody;
            }
            cubeInstance.velocity = Vector3.zero;
            cubeInstance.isKinematic = true;
            cubeInstance.transform.localScale = Vector3.zero;
            cubeInstance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Grabbable"));
        }
        else if (Input.GetAxis(triggerAxis) > 0.5f && cubeInstance != null)
        {
            if(axisAlignment)
            {
                cubeInstance.transform.localScale = transform.position - dragStartPosition;
            }
            else
            {
                cubeInstance.transform.localScale = Quaternion.Inverse(cubeInstance.transform.rotation)*transform.position - Quaternion.Inverse(cubeInstance.transform.rotation)*dragStartPosition;
            }
        }
        else
        {
            if(cubeInstance != null)
            {
                cubeInstance.isKinematic = false;
                cubeInstance = null;
            }
            hasStartedDrawing = false;
        }

        if(Input.GetButtonDown(toggleAxisAlignmentButton))
        {
            axisAlignment = !axisAlignment;
        }
    }
}
