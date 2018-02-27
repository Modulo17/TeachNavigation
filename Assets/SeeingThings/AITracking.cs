using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AITracking : MonoBehaviour {


    protected   int mIgnoredLayers; //Layers to Ignore, built on Start()

    public FakePhysics SeenObject;          //Object being seen, null if nothing

    public FakePhysics TrackedObject;       //Object being tracked, null if nothing

    public float ScanPerSeconds = 1.0f;

    public float    TrackPerSecond = 1.0f;
    public float    LooseSightAfterSeconds = 5.0f;

    public  float ScanDistance = 10.0f;

    public float mLostTimer;

    CharacterController mController;

    // Use this for initialization
    void Start () {
        mIgnoredLayers = GetIgnoredLayers(gameObject);
        mController = GetComponent<CharacterController>();
        StartCoroutine(ScanForEnemy());     //Find Enemy
        StartCoroutine(TrackEnemy());       //Track Enemy
    }


    //Periodically Scans for Enemy and sets SeenObject if found
    IEnumerator ScanForEnemy() {
        while(true) {
            SeenObject = CanSee(transform.position, transform.forward * ScanDistance);  //Try to find the enemy
            yield return new WaitForSeconds(1.0f / ScanPerSeconds);
        }
    }


    //Track Enemy every second while it can be seen, once sight lost, will null target after
    IEnumerator TrackEnemy() {
        mLostTimer = 0.0f;
        float mLastTime=Time.time;
        while(true) {
            float tInterval = 1.0f / TrackPerSecond;        //Seconds per interval
            if (SeenObject!=null) {         //Have we seen the Enemy?
                if (TrackedObject != SeenObject) {      //Is it the same one as last time
                    TrackedObject = SeenObject;
                    mLostTimer = 0.0f;          //Reset lost counter
                }
            } else {    //We can  no longer see the enemy
                if (mLostTimer > LooseSightAfterSeconds) {      
                    TrackedObject = null;       //Loose track
                } else {
                    mLostTimer += Time.time-mLastTime;
                }
            }
            mLastTime = Time.time;      //Keep track of current time so we can calc ellapsed time
            yield return new WaitForSeconds(tInterval);     //Let other stuff process
        }
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
