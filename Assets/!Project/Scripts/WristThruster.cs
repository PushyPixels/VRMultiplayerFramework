using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristThruster : MonoBehaviour
{
    public bool rightHand = false;
    public float thrusterStrength = 10.0f;
    public float megaThrustMultiplier = 5.0f;
    public float megaBrakesMultiplier = 5.0f;
    public ForceMode forceMode = ForceMode.Acceleration;
    public new Rigidbody rigidbody;

    private string leftHandButton = "XRI_Left_SecondaryButton";
    private string rightHandButton = "XRI_Right_SecondaryButton";
    private string leftHandButtonVD = "XRI_Left_PrimaryButton";
    private string rightHandButtonVD = "XRI_Right_PrimaryButton";

    private string selectedInput;
    private string sisterInput;

    void Start()
    {
        if (rightHand)
        {
            selectedInput = rightHandButton;
            sisterInput = leftHandButton;
        }
        else
        {
            selectedInput = leftHandButton;
            sisterInput = rightHandButton;
        }

        Debug.Log("I am using " + selectedInput,gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(selectedInput))
        {
            if (Input.GetButton(sisterInput)) // Both buttons down, time to megathrust/megabreak
            {
                float dotProduct = Vector3.Dot(rigidbody.velocity.normalized, transform.forward);
                if (dotProduct < 0) // We aren't facing our objective so use megabrakes (even stronger than megathrust)
                {
                    float megaBrakesValue = (megaBrakesMultiplier * Mathf.Abs(dotProduct));
                    if (megaBrakesValue < 1.0f)
                    {
                        megaBrakesValue = 1.0f;
                    }
                    rigidbody.AddForce(transform.forward * thrusterStrength * megaBrakesValue, forceMode);
                }
                else // Regular megathrust
                {
                    rigidbody.AddForce(transform.forward * thrusterStrength * megaThrustMultiplier, forceMode);
                }
            }
            else
            {
                rigidbody.AddForce(transform.forward * thrusterStrength, forceMode);
            }

            if (NetworkPlayer.instance != null)
            {
                if (rightHand)
                {
                    if (!NetworkPlayer.instance.rightThruster.isPlaying)
                    {
                        NetworkPlayer.instance.rightThruster.PlaySound();
                    }
                }
                else
                {
                    if (!NetworkPlayer.instance.leftThruster.isPlaying)
                    {
                        NetworkPlayer.instance.leftThruster.PlaySound();
                    }
                }
            }
        }
        else
        {
            if (NetworkPlayer.instance != null)
            {
                if (rightHand)
                {
                    if (NetworkPlayer.instance.rightThruster.isPlaying)
                    {
                        NetworkPlayer.instance.rightThruster.StopSound();
                    }
                }
                else
                {
                    if (NetworkPlayer.instance.leftThruster.isPlaying)
                    {
                        NetworkPlayer.instance.leftThruster.StopSound();
                    }
                }
            }
        }

        if(Input.GetButton("XRI_Left_PrimaryButton") || Input.GetButton("XRI_Right_PrimaryButton"))
        {
            rigidbody.velocity *= 0.98f;
        }
    }
}
