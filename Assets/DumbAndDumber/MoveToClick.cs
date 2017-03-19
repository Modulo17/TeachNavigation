using UnityEngine;
using System.Collections;



public class MoveToClick : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) { 
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = (Vector2)position;
        }
    }
}
