using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AITracking : MonoBehaviour {

    public   enum State {
        Invalid
        ,None
        ,Search
        ,TargetLocked
        ,TargetMemory
    }


    protected   int mIgnoredLayers; //Layers to Ignore, built on Start()

    public FakePhysics SeenObject;          //Object being seen, null if nothing

    public FakePhysics TrackedObject;       //Object being tracked, null if nothing


    public int      MaxLostCount = 50;

    public  float   ScanDistance = 10.0f;

    public int      mLostCount=0;

    CharacterController mController;

    [SerializeField]
    protected State mCurrentState = State.None;

    [SerializeField]
    protected State mPreviousState = State.Invalid;

    public  State   AIState {
        get {
            return mCurrentState;
        }
    }

    // Use this for initialization
    void    Start () {
        mIgnoredLayers = GetIgnoredLayers(gameObject);
        mController = GetComponent<CharacterController>();
        NewState(State.Search);
        StartCoroutine(RunStateMachine());
    }


    void    NewState(State vNewState) {
        if(vNewState!=mCurrentState) {
            ExitOldState(vNewState);
            EnterNewState(vNewState);
            mPreviousState = mCurrentState;
            mCurrentState = vNewState;
        }
    }

    void    ExitOldState(State vNewState) {
    }

    void    EnterNewState(State vNewState) {
        switch(vNewState) {

            case    State.Search:
                TrackedObject = null;
                SeenObject = null;
                break;

            case    State.TargetLocked:
                TrackedObject = SeenObject;
                mLostCount = MaxLostCount;
                break;

            case    State.TargetMemory:
                SeenObject = null;
                mLostCount = MaxLostCount;
                break;

        }
    }

    void    ProcessState() {
        switch(mCurrentState) {

            case    State.Search:
                SeenObject = CanSee(transform.position, transform.forward * ScanDistance);  //Try to find the enemy
                if(SeenObject!=null) {
                    NewState(State.TargetLocked);
                }
                break;

            case    State.TargetLocked: {
                    FakePhysics tObject= CanSee(transform.position, transform.forward * ScanDistance);  //Try to find the enemy
                    if(tObject==null ||  tObject!=SeenObject) {
                        NewState(State.TargetMemory);
                    }
                }
                break;

            case    State.TargetMemory: {
                    FakePhysics tObject = CanSee(transform.position, transform.forward * ScanDistance);  //Try to find the enemy
                    if (tObject == null || tObject != SeenObject)
                    {
                        if(mLostCount>0) {
                            mLostCount--;
                        } else {
                            NewState(State.Search);
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    IEnumerator RunStateMachine() {     //Run AI every 10th of a second
        while(true) {
            ProcessState();
            yield return new WaitForSeconds(1.0f/10.0f);
        }
    }

    public  override    string  ToString() {
        switch(mCurrentState) {
            case State.Search:
                return string.Format("No Lock");
            case State.TargetLocked:
                return string.Format("Visual Lock {0:s}",TrackedObject.name);
            case State.TargetMemory:
                return string.Format("Memory Lock {0:s} {1:d}", TrackedObject.name,mLostCount);
        }
        return "Invalid State";
    }


    //Use Raycast to find object in line of sight, using physics Layermask
    FakePhysics  CanSee(Vector3 vPosition, Vector3 vDirection) {
        RaycastHit tHit;
          if (Physics.Raycast(vPosition,vDirection,out tHit,vDirection.magnitude,mIgnoredLayers,QueryTriggerInteraction.Ignore)) {
            GameObject tGO = tHit.collider.gameObject;
            FakePhysics tFakePhysics = tGO.GetComponent<FakePhysics>();
            if (tFakePhysics!=null) {
                Debug.DrawLine(vPosition, tHit.point, Color.red, 1.0f);         //Viable Target
                return tFakePhysics;
            } else {
                Debug.DrawLine(vPosition, tHit.point, Color.yellow, 0.1f);      //Non viable target 
            }
        } else {
            Debug.DrawLine(vPosition, vPosition + vDirection, Color.grey, 0.1f);    //Nothing Hit
        }
        return null;
    }

    //Make up an Ignore mask based on Physics settings
    int GetIgnoredLayers(GameObject vGO) {
        int tLayer = vGO.layer;
        int tLayerIgnoreMask = 0;
        for (int i = 0; i < 32;  i++) {
            if (!Physics.GetIgnoreLayerCollision(tLayer, i)) {
                tLayerIgnoreMask = tLayerIgnoreMask | 1 << i;
            }
        }
        return tLayerIgnoreMask;
    }

}
