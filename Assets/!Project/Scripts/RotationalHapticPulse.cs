using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalHapticPulse : MonoBehaviour
{
    public float degreesPerSmallPulse = 5.0f;
    public float degreesPerLargePulse = 25.0f;

    public HapticsManager.HapticPulse largePulse;
    public HapticsManager.HapticPulse smallPulse;

    private Quaternion lastSmallPulseOrientation;
    private Quaternion lastLargePulseOrientation;

    void Start()
    {
        lastSmallPulseOrientation = transform.rotation;
        lastLargePulseOrientation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Quaternion.Angle(lastSmallPulseOrientation,transform.rotation) > degreesPerSmallPulse)
        {
            HapticsManager.instance.ApplyHapticImpulse(smallPulse);
            lastSmallPulseOrientation = transform.rotation;
        }
        if(Quaternion.Angle(lastLargePulseOrientation,transform.rotation) > degreesPerLargePulse)
        {
            HapticsManager.instance.ApplyHapticImpulse(largePulse);
            lastLargePulseOrientation = transform.rotation;
        }
    }
}
