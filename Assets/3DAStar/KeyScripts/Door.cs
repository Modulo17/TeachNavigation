using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Door : MonoBehaviour {

	public	Toggle	DoorToggle;

	public	float	OpenHeight;

	float	mClosedHeight;

	// Use this for initialization
	void Start () {
		mClosedHeight = transform.position.y;
		StartCoroutine (State ());
	}

	bool	isOpen {
		get {
			return	Mathf.Approximately (transform.position.y, OpenHeight);
		}
	}

	bool	isClosed {
		get {
			return	Mathf.Approximately (transform.position.y, mClosedHeight);
		}
	}

	IEnumerator	State() {
		while (true) {
			if (DoorToggle.isOn) {		//Open and shut door based on toggle
				while (!isOpen) {
					Vector3	tTarget = transform.position;
					tTarget.y = Mathf.Clamp (tTarget.y - 0.1f, Mathf.Min(OpenHeight, mClosedHeight), Mathf.Max(OpenHeight, mClosedHeight));
					transform.position = tTarget;
					yield return	new WaitForSeconds (0.1f);
				}
			} else {
				while (!isClosed) {
					Vector3	tTarget = transform.position;
					tTarget.y = Mathf.Clamp (tTarget.y + 0.1f, Mathf.Min(OpenHeight, mClosedHeight), Mathf.Max(OpenHeight, mClosedHeight));
					transform.position = tTarget;
					yield return	new WaitForSeconds (0.1f);
				}
			}
			yield return	new WaitForSeconds (0.1f);
		}
	}
}
