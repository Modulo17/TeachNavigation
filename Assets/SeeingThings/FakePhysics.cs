using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]
public class FakePhysics : MonoBehaviour {


    [SerializeField]
    private Text DebugText;      //Link in IDE


    //Get Controller at runtime
    protected CharacterController mController;

    [SerializeField] //Will Show in inspector
    protected   float JumpHeight = 10.0f;

    [SerializeField] //Will Show in inspector
    protected float MoveSpeed = 1.0f;

    [SerializeField] //Will Show in inspector
    protected float RotateSpeed = 10.0f;


    [SerializeField] //Will Show in inspector
    protected float GroundDrag = 0.2f;

    [SerializeField] //Will Show in inspector
    protected float AirDrag = 0.05f;



    //Used to move and rotate the character, this iwll be applied on Update
    protected   Vector3 mVelocity = Vector3.zero;

    protected   Vector3 mCurrentMoveInput=Vector3.zero;
    protected   Quaternion mCurrentRotationInput = Quaternion.identity;

    protected bool      mJumpInput=false;


    // Use this for initialization
    void Start() {
        mController = GetComponent<CharacterController>();
        StartPhysics();
    }

    protected virtual void StartPhysics() { }

    //Allows character to be moved by derived class
    protected virtual void UpdatePhysicsInput() {  }

    // Update is called once per frame
    void Update() {
        UpdatePhysicsInput();
        transform.rotation *= mCurrentRotationInput;
        mVelocity += mCurrentMoveInput;
        if(mJumpInput) {
            mVelocity.y += -Physics.gravity.y * JumpHeight;
        }
        if(!mController.isGrounded) {
            mVelocity += Physics.gravity;
        }
        mController.Move(transform.localRotation * mVelocity * Time.deltaTime);
        ApplyDrag();
    }

    //Apply air or ground drag
    protected void ApplyDrag() {
        if (mController.isGrounded) {
            mVelocity.x -= mVelocity.x * GroundDrag;      //Slow Down on ground
            mVelocity.z -= mVelocity.z * GroundDrag;      //Slow Down on ground
            mVelocity.y -= mVelocity.y * AirDrag;       //Slow Down in air
        } else {
            mVelocity.x -= mVelocity.x * AirDrag;      //Slow Down in air
            mVelocity.z -= mVelocity.z * AirDrag;      //Slow Down in air
            mVelocity.y -= mVelocity.y * AirDrag;      //Slow Down in air
        }
    }

    //Set text above Capsule
    protected void  SetDebugText(string vText, Color vColour) {
        if (DebugText != null) {
            DebugText.text = vText;
            DebugText.color = vColour;
        }
    }
    protected   void SetDebugText(string vText) {
        if (DebugText != null) {
            DebugText.text = vText;
        }
    }
}
