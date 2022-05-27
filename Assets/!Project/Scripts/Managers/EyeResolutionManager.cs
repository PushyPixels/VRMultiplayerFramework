using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EyeResolutionManager : MonoBehaviour
{
    public float eyeTextureResolutionScale = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        XRSettings.eyeTextureResolutionScale = eyeTextureResolutionScale;
    }
}
