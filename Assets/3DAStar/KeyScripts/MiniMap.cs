using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {

	public	Transform	Player;
	public	Transform	MiniMapCamera;

	public	Vector3	mPosition=Vector3.zero;

	// Update is called once per frame
	void Update () {
		mPosition.x = Player.position.x;
		mPosition.y = MiniMapCamera.position.y;
		mPosition.z = Player.position.z;
		MiniMapCamera.position = mPosition;
	}
}
