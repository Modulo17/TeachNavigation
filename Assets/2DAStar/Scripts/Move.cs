using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {


	IEnumerator	mSmoothMove;

	// Use this for initialization
	void Start () {

	}

	IEnumerator MoveTo(Vector3 vNewPosition, float vTime) {
		float	tStart = Time.fixedTime;
		float	tD = 0.0f;
		Vector3	tFrom = transform.position;
		float	tDistance = Vector3.Magnitude (vNewPosition - transform.position);
		vTime *= tDistance;
		while(tDistance>Mathf.Epsilon) {
			transform.position = Vector3.Lerp (tFrom, vNewPosition, tD/vTime);
			tDistance = Vector3.Magnitude (vNewPosition - transform.position);
			tD += Time.deltaTime;
			yield	return	null;
		}
		transform.position = vNewPosition;
		mSmoothMove = null;
		Debug.Log(string.Format("took {0} seconds",Time.fixedTime-tStart));
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Vector3	position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			position.x = Mathf.RoundToInt (position.x);
			position.y = Mathf.RoundToInt (position.y);
			position.z = 0.0f;
			if (mSmoothMove == null) {
				mSmoothMove = MoveTo (position,1.0f);
				StartCoroutine(mSmoothMove);
			}
		}
	}
}
