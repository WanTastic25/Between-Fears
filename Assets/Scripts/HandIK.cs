using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    private Animator animator;
    public Transform leftHandIKTarget;  // The position where you want to position the left hand
    public Transform flashlight;        // The flashlight's transform (where the hand should hold it)

    void Start()
    {
        animator = GetComponent<Animator>();  // Get the Animator attached to the player
    }

    // Called by Unity to apply IK during the animation process
    private void OnAnimatorIK(int layerIndex)
    {
        if (flashlight != null && leftHandIKTarget != null)
        {
            // Set the weight for the IK system to take control of the left hand
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);// Full control of the left hand's position
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);// Full control of the left hand's rotation

            // Set the position and rotation of the left hand to match the flashlight's position and rotation
            animator.SetIKPosition(AvatarIKGoal.LeftHand, flashlight.position);// Position the left hand at the flashlight
            animator.SetIKRotation(AvatarIKGoal.LeftHand, flashlight.rotation);// Rotate the left hand at the flashlight
        }
    }
}
