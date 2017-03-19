using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour {

    Text            mText;
    List<WayPoint>  mWayPointList;                 //parent list

    Camera mMainCamera;

    void Awake() {
        mText = GetComponentInChildren<Text>();
        mMainCamera = Camera.main;
    }

    public void Add(List<WayPoint> vWPlist,Vector3 vDestination) {
        transform.position = vDestination;
        mWayPointList = vWPlist;
        mWayPointList.Add(this);
    }

    public void Remove() {
        mWayPointList.Remove(this);
        Destroy(gameObject);
    }

    void Update() {
		if (mWayPointList != null) {
			mText.text = string.Format("Waypoint {0}", mWayPointList.IndexOf(this) + 1);
		}
        //transform.LookAt(transform.position + mMainCamera.transform.rotation * Vector3.forward, mMainCamera.transform.rotation * Vector3.up);        //Turn object towards camera
    }
}
