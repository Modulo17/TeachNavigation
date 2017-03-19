using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]

public class MovePlayerAgent : MonoBehaviour {

    public  Text DebugText;

    WayPointList    mWP;

    // Use this for initialization
    void Start () {
        mWP = GetComponent<WayPointList>();
    }

    private void Update() {
        if(GM.Selected == mWP) {
            DebugText.text = string.Format("Ready {0} WP to go", mWP.Count);
        } else {
            DebugText.text = string.Format("{0} WP to go",mWP.Count);
        }
    }
}
