using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]

public class WayPointList : MonoBehaviour {

    List<WayPoint> mList = new List<WayPoint>();
    public GameObject WaypointPrefab;

    NavMeshAgent mNA;

    public int Count {
        get {
            return mList.Count;
        }
    }

    // Use this for initialization
    void Start () {
        mNA = GetComponent<NavMeshAgent>();
	}

    public  void    Add(Vector3 vDestination,Color vColour) {
        GameObject tGO = Instantiate(WaypointPrefab) as GameObject;
        WayPoint tWP = tGO.GetComponent<WayPoint>();
        tGO.GetComponentInChildren<MeshRenderer>().material.color = vColour;
        tWP.Add(mList, vDestination);
        if(mList.Count==1) {        //do first one if this is only one, rest will then sequence
			mNA.SetDestination(tWP.transform.position);
        }
    }

    private void Update() {
        if(mList.Count>0) {
			if(Arrived) {
                WayPoint tWP=mList[0];
                tWP.Remove();
                if(mList.Count>0) {
					mNA.SetDestination(mList[0].transform.position);
                }
            }
        }
    }

	bool	Arrived {
		get {
			if(!mNA.pathPending)
			{
				return mNA.remainingDistance<0.5;
			}
			return	false;
		}
	}

}
