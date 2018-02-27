using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayerCharacter : FakePhysics {


    //Update Character movement based on control input
    protected override void UpdatePhysicsInput() {
        mCurrentRotationInput = Quaternion.Euler(0, GetRotationControl() * RotateSpeed, 0);
        mCurrentMoveInput.x = Input.GetAxis("Horizontal") * MoveSpeed;
        mCurrentMoveInput.y = 0.0f;
        mCurrentMoveInput.z = Input.GetAxis("Vertical") * MoveSpeed;
        mJumpInput = mController.isGrounded && Input.GetButton("Jump");
    }
    //Get rotation either from mouse or XBox controller, not a great hack!
    float GetRotationControl() {
        //Really poor way of doing this, but Unity has no easy way to check if an axis has been set up in Input
        //It just crashes the script, this catches the crash and sues the mosue if there is no Xbox controller set up
        try {
            float tControl = Input.GetAxis("Horizontal1");      //If XBox controller has input use this
            if (Mathf.Abs(tControl) > Mathf.Epsilon) return tControl;
            else  return Input.GetAxis("Mouse X");      //If not use Mouse X
        }
        catch {
            return Input.GetAxis("Mouse X");        //If the check for the XBox axis caused an excpetion catch it and use Mouse
        }
    }
}
