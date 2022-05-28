using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStabilizer : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        //transform.eulerAngles = Vector3.Scale(transform.eulerAngles, new Vector3(1.0f,1.0f,0.0f));
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward,Vector3.up),Vector3.up);
    }
}
