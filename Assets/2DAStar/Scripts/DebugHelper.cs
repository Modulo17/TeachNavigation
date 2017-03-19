using UnityEngine;
using System.Collections;

public class DebugHelper : MonoBehaviour {

	// Use this for initialization

	static	DebugHelper		DH;
	static	GameObject		mDotObject;

	void Awake() {

		// Use this for initialization
		if (DH == null) { //If we dont have it already, make it persistent
			DH = this;
			DontDestroyOnLoad (gameObject);    //Don't unload it
			mDotObject=Resources.Load("DebugDot") as GameObject;
			if (mDotObject != null) {
			} else {
				Debug.Log ("Could not load prefab");
			}
		} else if (DH != this) {
			Destroy (this.gameObject);       //If created more than once kill it, we only want one InputController
		}
	}

	static public	void	DebugDot(int vX,int vY, Color vColor,float vTime=1.0f) {
		DebugDotScript	tDebugDotScript;
		tDebugDotScript = mDotObject.GetComponent<DebugDotScript> ();
		tDebugDotScript.DotColour = vColor;
		tDebugDotScript.DotDuration = vTime;
		GameObject.Instantiate (mDotObject, new Vector2 (vX, vY), Quaternion.identity,DH.transform);
	}
}
