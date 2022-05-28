using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 

public class EnableHumanoidIK : MonoBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public GameObject rootObject;

    // AvatarIKHints can be used for more places (elbows etc)
    // Also can set look direction

    public Animator animator;
    private HumanDescription humanDescription;

    void OnValidate () 
    {
        if(animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        if(rootObject == null)
        {
            rootObject = gameObject;
        }
    }

    void Start()
    {
        humanDescription = animator.avatar.humanDescription;
        humanDescription.armStretch = 2.0f;
        Avatar newAvatar = AvatarBuilder.BuildHumanAvatar(rootObject,humanDescription);
        if(newAvatar.isValid)
        {
            animator.avatar = newAvatar;
        }
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
        if(leftHandTarget != null)
        {
            SetAllIKForGoal(AvatarIKGoal.LeftHand,leftHandTarget);
        }
        if(rightHandTarget != null)
        {
            //Debug.Log("Doin' that IK stuff boss");
            SetAllIKForGoal(AvatarIKGoal.RightHand, rightHandTarget);
        }
        if(leftFootTarget != null)
        {
            SetAllIKForGoal(AvatarIKGoal.LeftFoot, leftFootTarget);
        }
        if(rightFootTarget != null)
        {
            SetAllIKForGoal(AvatarIKGoal.RightFoot,rightFootTarget);
        }
                
        // Look stuff for later
        //if(lookObj != null) {
        //    animator.SetLookAtWeight(1);
        //    animator.SetLookAtPosition(lookObj.position);
        //}    
    }    
}
