using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using RL_Helpers;

public class GM : Singleton {

    static GM sGM;

    WayPointList mCurrentSelected;      //Keep tabs on which one is currently selected

    public  GameObject AgentPrefab;

    public static WayPointList  Selected {      //Get selected Agent 
        get {
            if(sGM!=null) {
                return sGM.mCurrentSelected;
            }
            return  null;
        }
   }

    void Awake() {
        if (CreateSingleton<GM>(ref sGM)) {
        }
    }
    // Update is called once per frame
    void Update() {
        CheckClicked();
    }

    void CheckClicked() {       //Handle raycast, if we hit an agent select them , if not add waypoints to previously selected agent
		if(!EventSystem.current.IsPointerOverGameObject()) {
		RaycastHit tHit;
			if (Input.GetMouseButtonDown (0)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out tHit)) {
					WayPointList tWPList = tHit.collider.gameObject.GetComponent<WayPointList> ();
					Debug.DrawRay (tHit.point, Vector3.up * 3f, Color.red);      //Debug
					if (tWPList != null) {                     //If this is an agent, then we can select it
						mCurrentSelected = tWPList;
					} else {                            //If not add we have one selected from before add more waypoints
						if (mCurrentSelected != null) {
							Color tColour = mCurrentSelected.gameObject.GetComponentInChildren<MeshRenderer> ().material.color;
							mCurrentSelected.Add (tHit.point, tColour);
						}
					}
				}
			}
        }
    }

    public static void AddAgent() {     //Add a new agent
        float tSize = 20f;
        Vector3 tPosition = new Vector3(Random.Range(-tSize, tSize),20f, Random.Range(-tSize, tSize));
        GameObject tGO=Instantiate(sGM.AgentPrefab);
        MeshRenderer tMR = tGO.GetComponent<MeshRenderer>();
        tMR.material.color = Random.ColorHSV();
        tGO.transform.position = tPosition;
    }

}