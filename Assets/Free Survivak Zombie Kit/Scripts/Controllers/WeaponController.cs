using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public PlayerController playerController;
    public Animator weaponAnimator;

    void Update()
    {
        UpdateWeaponAnimation();
    }

    private void UpdateWeaponAnimation()
    {
        // Determine the current motion state and set the animation parameters accordingly
        switch (playerController.currentMotion)
        {
            case PlayerController.MotionState.Idle:
                SetWeaponAnimationState(false); // Set idle state
                break;

            case PlayerController.MotionState.Running:
                SetWeaponAnimationState(true); // Set running state
                break;

            case PlayerController.MotionState.Jumping:
                // Optional: Add a jumping animation if needed
                // weaponAnimator.SetBool("jumping", true);
                break;

            default:
                // Optional: Reset all animations if no known state
                ResetWeaponAnimation();
                break;
        }
    }

    private void SetWeaponAnimationState(bool isRunning)
    {
        weaponAnimator.SetBool("running", isRunning);
    }

    private void ResetWeaponAnimation()
    {
        weaponAnimator.SetBool("running", false);
        // Reset other animation states if necessary
    }
}
