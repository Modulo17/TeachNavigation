using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

    public bool FaceCamera;             //Have this object face the camera
    public bool NoParentRotation;       //Undo Parent rotation


    Camera mMainCamera;
    Quaternion mRotation;

    void LateUpdate() {        //Undo Parent rotation
        if(NoParentRotation) {
			transform.rotation=mRotation;     //Restore intial rotation
        }
    }

    // Use this for initialization
    void Start () {
        mMainCamera = Camera.main;
        mRotation = transform.rotation;     //Keep intial rotation
    }

    // Update is called once per frame
    void Update () {
        if(FaceCamera) {
            transform.LookAt(transform.position + mMainCamera.transform.rotation * Vector3.forward, mMainCamera.transform.rotation * Vector3.up);        //Turn object towards camera
        }
    }
}
