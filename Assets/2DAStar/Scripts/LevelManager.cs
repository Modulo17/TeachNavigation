using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public	GameObject[,]	Map;

	public 	TextAsset LevelDesign;

	public	GameObject[]		Pickups;
	public	GameObject[]		Walls;

	GameObject[,]	CreateRoom(string vRoomCSV) {
		int	tH = 1;
		int tW = 0;
		int	tMaxWidth = 0;
		foreach (var tChar in vRoomCSV) {
			switch (tChar) {
			case '\r':
				tW++;
				tMaxWidth = Mathf.Max (tMaxWidth, tW);
				tW = 0;
				tH++;
				break;
			case ',':
				tW++;
				break;
			}
		}
		return	new GameObject[tMaxWidth, tH];
	}


	GameObject[,]	LoadRoom(string vRoomCSV) {
		var	tMap = CreateRoom(vRoomCSV);
		int	tWidth = tMap.GetLength (0);
		int	tHeight = tMap.GetLength (1);
		int	tH = 0;
		int tW = 0;
		foreach (var tChar in LevelDesign.text) {
			switch (tChar) {
			case '\r':
				tW = 0;
				tH++;
				break;
			case ',':
				tW++;
				break;
			case '1': {
					int tInvHeight = (tHeight - tH) - 1;	//Invert Height as Unity 0 is bottom
					tMap [tW, tInvHeight] = WallAt (tW, tInvHeight);
				}
				break;
			}
		}
		return	tMap;
	}


	// Use this for initialization
	void Start () {
		Map = LoadRoom (LevelDesign.text);
	}


    GameObject    WallAt(int vX, int vY) {
        GameObject tGO = (GameObject)Instantiate(Walls[0], transform);
        tGO.transform.position = new Vector2(vX, vY);
        tGO.transform.rotation = Quaternion.identity;
		return tGO;
    }

}
