using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroRotationOnAndroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            transform.localRotation = Quaternion.identity;
        }
    }
}
