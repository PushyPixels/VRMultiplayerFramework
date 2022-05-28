using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialVRMovement : MonoBehaviour
{
    public float maxSpeed = 1.0f;
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";
    public bool threeDMovement = false;

    [Header("RightStickStuff")]
    public float turnSpeed = 180.0f;
    public float snapAngle = 60.0f;
    public bool snapTurn;
    public string rotationAxisName = "XRI_Right_Primary2DAxis_Horizontal";

    [Header("Required Scene/Child References")]
    public Transform offhandMovementHand;

    [Header("Debug settings")]
    public bool logErrorsAfterInitialFailure = false;

    private bool hasThrownError;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!offhandMovementHand)
        {
            LogError();
            return;
        }

        Vector3 verticalMovementVector = offhandMovementHand.forward;

        if (!threeDMovement)
        {
            // Project offhandMovementHand forward axis onto floor plane so that the player doesn't fly
            // Also, normalize (so that lifting controller up and down does not affect magnitude)
            verticalMovementVector = Vector3.ProjectOnPlane(verticalMovementVector, Vector3.up).normalized;
        }

        Vector3 horizontalMovementVector = Vector3.Cross(verticalMovementVector,-Vector3.up);

        GetComponent<Rigidbody>().MovePosition(transform.position + verticalMovementVector * Input.GetAxis(verticalAxisName) * maxSpeed * Time.deltaTime +
            horizontalMovementVector * Input.GetAxis(horizontalAxisName) * maxSpeed * Time.deltaTime);

        //transform.position += verticalMovementVector * Input.GetAxis(verticalAxisName) * maxSpeed * Time.deltaTime;
        //transform.position += horizontalMovementVector * Input.GetAxis(horizontalAxisName) * maxSpeed * Time.deltaTime;

        transform.eulerAngles += new Vector3(0.0f,Input.GetAxis(rotationAxisName)*turnSpeed*Time.deltaTime,0.0f);
    }

    void LogError()
    {
        if(logErrorsAfterInitialFailure || !hasThrownError)
        {
            Debug.LogError("offhandMovementHand reference not set!", gameObject);
            hasThrownError = true;
        }
        return;
    }
}
