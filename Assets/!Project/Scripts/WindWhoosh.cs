using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindWhoosh : MonoBehaviour
{
    public float triggerVelocity = 25.0f;
    public float maximumVelocity = 100.0f;

    public float maximumVolume = 1.0f;

    public new Rigidbody rigidbody;
    public new AudioSource audio;

    // Update is called once per frame
    void Update()
    {
        float velocity = rigidbody.velocity.magnitude;

        if(!audio.isPlaying)
        {
            audio.Play();
        }
        audio.volume = Mathf.Lerp(0.0f,maximumVolume,(velocity-triggerVelocity)/(maximumVelocity-triggerVelocity));
    }
}
