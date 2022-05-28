using UnityEngine;
using System;
using System.Collections;
using Mirror;

[RequireComponent(typeof(Animator))] 

public class TaggedHumanoidIKNet : NetworkBehaviour
{
    public string leftHandTarget;
    public string rightHandTarget;
    public string leftFootTarget;
    public string rightFootTarget;

    private Transform leftHandTargetTransform;
    private Transform rightHandTargetTransform;
    private Transform leftFootTargetTransform;
    private Transform rightFootTargetTransform;

    // AvatarIKHints can be used for more places (elbows etc)
    // Also can set look direction

    public Animator animator;

    void OnValidate() 
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        leftHandTargetTransform = GameObject.FindGameObjectWithTag(leftHandTarget).transform;
        rightHandTargetTransform = GameObject.FindGameObjectWithTag(rightHandTarget).transform;
        leftFootTargetTransform = GameObject.FindGameObjectWithTag(leftFootTarget).transform;
        rightFootTargetTransform = GameObject.FindGameObjectWithTag(rightFootTarget).transform;
    }

    void SetAllIKForGoal(AvatarIKGoal goal, Transform target, float weight = 1.0f)
    {
        animator.SetIKPositionWeight(goal,weight);
        animator.SetIKRotationWeight(goal,weight);  
        animator.SetIKPosition(goal,target.position);
        animator.SetIKRotation(goal,target.rotation);
    }
    
    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(hasAuthority)
        { 
            if(leftHandTarget != null)
            {
                SetAllIKForGoal(AvatarIKGoal.LeftHand,leftHandTargetTransform);
            }
            if(rightHandTarget != null)
            {
                SetAllIKForGoal(AvatarIKGoal.RightHand, rightHandTargetTransform);
            }
            if(leftFootTarget != null)
            {
                SetAllIKForGoal(AvatarIKGoal.LeftFoot, leftFootTargetTransform);
            }
            if(rightFootTarget != null)
            {
                SetAllIKForGoal(AvatarIKGoal.RightFoot,rightFootTargetTransform);
            }
        }
                
        // Look stuff for later
        //if(lookObj != null) {
        //    animator.SetLookAtWeight(1);
        //    animator.SetLookAtPosition(lookObj.position);
        //}    
    }    
}
