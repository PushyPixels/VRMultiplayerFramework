using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrabMovement : MonoBehaviour
{
    public HandInfo leftHand;
    public HandInfo rightHand;
    public LayerMask grabbableObjectLayerMask = 0;
    public LayerMask movementObjectLayerMask = -1;
    public float minimumMovementDistance = 0.01f;

    public int framesToAverageWhenThrowing = 3;

    [HideInInspector]
    public float sqrMinimumMovementDistance;

    public NetworkConnection client;

    [System.Serializable]
    public class HandInfo
    {
        [Header("Required Object References")]
        public Rigidbody rigidbody;
        public string gripAxis;

        [Header("Debug Info")]
        public HandState state;
        public Vector3 grabOffset;
        public Rigidbody grippedObject;
        public int grippedObjectOriginalLayer;
        public Deque<Vector3> previousPostions = new Deque<Vector3>();
        public bool skip;
    }
    public enum HandState { Empty, MovementGrab, ObjectGrab }

    [Header("Required Component References")]
    public new Rigidbody rigidbody;

    private int dequeueCount;
    private Vector3 previousVelocity;

    // Update is called once per frame
    void Update()
    {
        ObjectGrabbingLogic(leftHand);
        ObjectGrabbingLogic(rightHand);

        WorldMovementLogic(leftHand, rightHand);
        WorldMovementLogic(rightHand, leftHand);

        if (leftHand.skip && Input.GetAxis(leftHand.gripAxis) < 0.25f)
        {
            leftHand.skip = false;
        }
        if (rightHand.skip && Input.GetAxis(rightHand.gripAxis) < 0.25f)
        {
            rightHand.skip = false;
        }

        leftHand.previousPostions.Enqueue(leftHand.rigidbody.transform.position);
        rightHand.previousPostions.Enqueue(rightHand.rigidbody.transform.position);

        dequeueCount--;
        if(dequeueCount < 0)
        {
            leftHand.previousPostions.Dequeue();
            rightHand.previousPostions.Dequeue();
        }
    }

    void ObjectGrabbingLogic(HandInfo hand)
    {
        if (Input.GetAxis(hand.gripAxis) >= 0.25f)
        {
            Collider[] colliders = Physics.OverlapSphere(hand.rigidbody.transform.position,
                                                         hand.rigidbody.transform.lossyScale.x/2.0f,
                                                         grabbableObjectLayerMask);
            if (colliders.Length > 0 && hand.state == HandState.Empty)
            {
                Collider firstColliderFound = colliders[0];
                Rigidbody rb = firstColliderFound.attachedRigidbody;

                if (rb != null)
                {
                    // Networking stuff, player requests authority over what they grab
                    SettableAuthority sAuth = rb.GetComponent<SettableAuthority>();
                    if(sAuth)
                    {
                        sAuth.SetAuthority();
                    }

                    hand.grippedObjectOriginalLayer = rb.gameObject.layer;

                    rb.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Grabbed"));

                    GrabAnchor grabAnchor = rb.GetComponentInChildren<GrabAnchor>();

                    if(grabAnchor)
                    {
                        grabAnchor.SetPositionRotation(hand.rigidbody.transform.position, hand.rigidbody.transform.rotation);
                    }

                    hand.grabOffset = rb.transform.position - hand.rigidbody.transform.position;

                    hand.grippedObject = rb;
                    FixedJoint joint = rb.gameObject.AddComponent<FixedJoint>();
                    joint.enablePreprocessing = false;
                    joint.connectedBody = hand.rigidbody;
                    hand.state = HandState.ObjectGrab;

                    if(hand == rightHand)
                    {
                        HapticsManager.instance.ApplyHapticImpulse(true,0.5f,0.15f);
                    }
                    else
                    {
                        HapticsManager.instance.ApplyHapticImpulse(false,0.5f,0.15f);
                    }
                    // hand.rigidbody.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }
            // Failed attempt at fixing object position when moving (also tried LateUpdate)
            //else if(hand.state == HandState.ObjectGrab)
            //{
            //    hand.grippedObject.position = hand.rigidbody.position + hand.grabOffset;
            //}
        }
        if (hand.state == HandState.ObjectGrab && Input.GetAxis(hand.gripAxis) < 0.25f)
        {
            Rigidbody rb = hand.grippedObject;
            FixedJoint joint = rb.GetComponent<FixedJoint>();
            Destroy(joint);

            if (rb != null)
            {
                rb.velocity = (hand.rigidbody.transform.position - hand.previousPostions.PeekFront()) / Time.fixedDeltaTime/(float)framesToAverageWhenThrowing;
                rb.gameObject.SetLayerRecursively(hand.grippedObjectOriginalLayer);
            }

            hand.state = HandState.Empty;
            //hand.rigidbody.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        previousVelocity = rigidbody.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we're gripping with a free hand, and we aren't already move gripping  with other hand (So you can't teleport yourself by hitting your head)
        if(Input.GetAxis(leftHand.gripAxis) >= 0.25f && leftHand.state == HandState.Empty && rightHand.state != HandState.MovementGrab)
        {
            Vector3 handVector = leftHand.rigidbody.transform.position - transform.position;

            // Drop velocity; player wants to grip after all
            rigidbody.velocity = Vector3.zero;
            transform.position = collision.GetContact(0).point;
            transform.position += handVector;

            if(leftHand.state != HandState.MovementGrab) // So haptics doesn't buzz continuously
            {
                HapticsManager.instance.ApplyHapticImpulse(false,0.65f,0.2f);
            }

            // Pretend it's a valid grip
            leftHand.state = HandState.MovementGrab;
        }

        // Same deal but on right side
        if(Input.GetAxis(rightHand.gripAxis) >= 0.25f && rightHand.state == HandState.Empty && leftHand.state != HandState.MovementGrab)
        {
            Vector3 handVector = rightHand.rigidbody.transform.position - transform.position;

            // Drop velocity; player wants to grip after all
            rigidbody.velocity = Vector3.zero;
            transform.position = collision.GetContact(0).point;
            transform.position += handVector;

            if(rightHand.state != HandState.MovementGrab) // So haptics doesn't buzz continuously
            {
                HapticsManager.instance.ApplyHapticImpulse(true,0.65f,0.2f);
            }

            // Pretend it's a valid grip
            rightHand.state = HandState.MovementGrab;
        }
    }

    void WorldMovementLogic(HandInfo hand, HandInfo oppositeHand)
    {
        if (!hand.skip && hand.state != HandState.ObjectGrab)
        {
            if (Input.GetAxis(hand.gripAxis) >= 0.25f &&
                (hand.state == HandState.MovementGrab || // If we don't check for this, auto-grab works badly
                Physics.CheckSphere(hand.rigidbody.transform.position,
                                    hand.rigidbody.transform.lossyScale.x/2.0f,
                                    movementObjectLayerMask)))
            {

                rigidbody.velocity = Vector3.zero;

                // Something new to try: affix player hand using fixedjoint (doesn't need an anchor)
                // Wait that won't work unless I change heirarchy (hands are currently free)
                // I spawned them because I had trouble with child transform stuff, because I approached it in reverse
                // I think I can do that the right way now with the hands pre-spawned as child transforms and see if that helps with collision.

                // Stuff below I tried for blocking head movement through walls
                //rigidbody.velocity = (hand.previousPostions.PeekBack() - hand.rigidbody.transform.position) / Time.fixedDeltaTime;
                //rigidbody.AddForce((hand.previousPostion - hand.rigidbody.transform.position) / Time.deltaTime, ForceMode.VelocityChange);
                //rigidbody.MovePosition(rigidbody.position + (hand.previousPostion - hand.rigidbody.transform.position));
                transform.position += hand.previousPostions.PeekBack() - hand.rigidbody.transform.position;
                
                if(hand.state != HandState.MovementGrab) // So haptics doesn't buzz continuously
                {
                    if(hand == rightHand)
                    {
                        HapticsManager.instance.ApplyHapticImpulse(true,0.65f,0.2f);
                    }
                    else
                    {
                        HapticsManager.instance.ApplyHapticImpulse(false,0.65f,0.2f);
                    }
                }

                hand.state = HandState.MovementGrab;

                if (oppositeHand.state == HandState.MovementGrab)
                {
                    oppositeHand.skip = true;
                    oppositeHand.state = HandState.Empty;
                }
            }
            if (hand.state == HandState.MovementGrab && Input.GetAxis(hand.gripAxis) < 0.25f)
            {
                Vector3 velocityVector = (hand.previousPostions.PeekFront() - hand.rigidbody.transform.position);
                if (velocityVector.sqrMagnitude > sqrMinimumMovementDistance)
                {
                    rigidbody.velocity = velocityVector / Time.fixedDeltaTime;
                }
                else
                {
                    rigidbody.velocity = Vector3.zero;
                }
                hand.state = HandState.Empty;
            }
        }
    }

    void OnValidate()
    {
        rigidbody = GetComponent<Rigidbody>();
        sqrMinimumMovementDistance = minimumMovementDistance * minimumMovementDistance;

        if(framesToAverageWhenThrowing <= 0)
        {
            framesToAverageWhenThrowing = 1;
        }
    }

    void Start()
    {
        dequeueCount = framesToAverageWhenThrowing;
    }
}
