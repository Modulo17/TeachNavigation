using UnityEngine;
using System.Collections;

public class InitialiseCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera tCamera = GetComponent<Camera> ();
		transform.position = new Vector3 (tCamera.orthographicSize * tCamera.aspect, tCamera.orthographicSize,-10.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
