using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class MoveZombie : SpriteHelper {

	public	float	Speed=1.0f;
	public	GameObject		Player;


	public	LevelManager	LevelManager;
	private List<PathFind.Waypoint> mWayPoints;

	protected	IEnumerator	mSmoothMove;

	PathFind	mPathFind;

	float		sTimeout=0.0f;

	GameObject[,]	mMap;

	protected	override	void	Awake() {
		base.Awake ();
		mPathFind = GetComponent<PathFind>();
	}

	void	Start() {
	}

	// Update is called once per frame
	void Update () {
		if (Player) {
			sTimeout -= Time.deltaTime;
			if (sTimeout <= 0.0f) {
				mMap = LevelManager.GetComponent<LevelManager> ().Map;
				sTimeout = UnityEngine.Random.Range (1, 7);
				PathFind.Waypoint tFrom = new PathFind.Waypoint (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y));
				PathFind.Waypoint tTo = new PathFind.Waypoint ( Mathf.RoundToInt (Player.transform.position.x), Mathf.RoundToInt (Player.transform.position.y));
				mWayPoints= mPathFind.Find(tFrom, 
					tTo,
					mMap);
				NextNavNode ();
			}
		}
	}

	bool NextNavNode() {
		if (mWayPoints !=null && mWayPoints.Count > 0) {
			PathFind.Waypoint mNode = mWayPoints.First<PathFind.Waypoint>();
			mWayPoints.Remove(mNode);
			MoveTo(new Vector2(mNode.X, mNode.Y));
			return false;
		} else {
			mWayPoints = null;
			return true;
		}
	}



	void	MoveTo(Vector2 vPosition) {
		if (mSmoothMove != null) {
			StopCoroutine (mSmoothMove);
			mSmoothMove = null;
		}
		mSmoothMove = MoveTo (vPosition,Speed,Arrived);
		StartCoroutine(mSmoothMove);
	}

	void	Arrived() {
		if(NextNavNode()) {
		} else {
		}
	}
}
