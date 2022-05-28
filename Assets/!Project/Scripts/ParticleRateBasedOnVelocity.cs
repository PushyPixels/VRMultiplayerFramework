using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRateBasedOnVelocity : MonoBehaviour
{
    public Rigidbody target;
    public ParticleSystem system;
    public float minimumVelocity = 0.0f;
    public float ratio = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.EmissionModule emission = system.emission;
        float velocity = target.velocity.magnitude;
        if(velocity > minimumVelocity)
        {
            emission.rateOverTime = target.velocity.magnitude * ratio;
        }
        else
        {
            emission.rateOverTime = 0.0f;
        }
    }
}
