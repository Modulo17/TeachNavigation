using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour {

    NavMeshAgent mAgent;

    public  GameObject Target;

	// Use this for initialization
	void Start () {
        mAgent = GetComponent<NavMeshAgent>();
        if (Target != null) {
            mAgent.destination = Target.transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
