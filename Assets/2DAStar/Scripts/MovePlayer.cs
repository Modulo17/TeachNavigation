using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class MovePlayer : SpriteHelper {

	public	LevelManager	LevelManager;

	public	float	Speed=1.0f;

	public	GameObject	Target;

	private	GameObject	mGoTo;

	private List<PathFind.Waypoint> mWayPoints;

	protected	IEnumerator	mSmoothMove;

	PathFind	mPathFind;

	protected	override	void	Awake() {
		base.Awake ();
		mGoTo = null;
        mPathFind = GetComponent<PathFind>();
	}


    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Fire1")) {
			GameObject[,]	tMap = LevelManager.GetComponent<LevelManager> ().Map;
			Vector3	position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            int tX = Mathf.RoundToInt(position.x);
            int tY = Mathf.RoundToInt(position.y);
            Target.transform.position = new Vector3(tX,tY,0);
			PathFind.Waypoint tFrom = new PathFind.Waypoint (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y));
			PathFind.Waypoint tTo = new PathFind.Waypoint (tX,tY);
			mWayPoints= mPathFind.Find(tFrom, 
		                            tTo,
									tMap,
                                    true);  //Debug on
            NextNavNode();
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
