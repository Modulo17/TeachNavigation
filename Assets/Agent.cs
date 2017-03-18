using UnityEngine;
using System.Collections;


[RequireComponent(typeof(NavMeshAgent))]
	
public class NavAgent : MonoBehaviour {

	NavMeshAgent mNMA;

	// Update is called once per frame
	void Update () {
	
	}
	public Transform goal;

	bool	isPlayer=false;

	void Start () {
		mNMA = GetComponent<NavMeshAgent>();
		isPlayer = (TrackPlayer == null);		//If you want this to be the player don't set tracking
		if (!isPlayer) {						//If NPC track player
			StartCoroutine (TrackPlayer (1f));
		}
	}

	void	Update() {
	}

	IEnumerator	TrackPlayer(float vTime) {
		do {
			mNMA.destination = goal.position; 
			yield	return	new	WaitForSeconds (vTime);
		} while(true);
	}

}
