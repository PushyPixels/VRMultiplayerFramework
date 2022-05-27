using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slaptics : MonoBehaviour
{
    public float activationVelocity = 0.1f;
    public float maxVelocity = 100.0f;
    public float minAmplitude = 0.1f;
    public float maxAmplitude = 1.0f;
    public float minDuration = 0.01f;
    public float maxDuration = 0.3f;
    public bool isRight;

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        if(speed > activationVelocity)
        {
            float velocityRatio = (speed-activationVelocity)/maxVelocity;

            // Lerp = LinEaR interPolation
            HapticsManager.instance.ApplyHapticImpulse(isRight,
                Mathf.Lerp(minAmplitude, maxAmplitude, velocityRatio),
                Mathf.Lerp(minDuration,  maxDuration,  velocityRatio));
        }
    }
}
