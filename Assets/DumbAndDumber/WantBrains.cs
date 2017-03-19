using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WantBrains))]

public class WantBrains : MonoBehaviour {

    public GameObject Target;       //Link player here
    public float mSpeed = 10f;
    public bool  Arrived;
    public bool  Auto;          //Track automatically when started

    Rigidbody2D mRB;

    Coroutine mTracking;

    void Start () {
        mRB = GetComponent<Rigidbody2D>();
        if(Auto) {
            StartCoroutine(AutoTrack(Target));
        }
	}

    IEnumerator AutoTrack(GameObject vTarget) {
        while(Auto) {
            if (mTracking == null) {
                NewTarget(Target);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void     NewTarget(GameObject vTarget) {
        CancelTrack();
        Target = vTarget;
        mTracking = StartCoroutine(FindTarget(Target));
    }

    public  void    CancelTrack() {
        if (mTracking != null) {           //Stop old tracking and start new
            StopCoroutine(mTracking);
            mTracking = null;
        }
    }

    IEnumerator FindTarget(GameObject vTarget) {
        Vector2 tPath = vTarget.transform.position - transform.position;
        Arrived = false;
        do {
            Vector2 tNextStep= tPath.normalized * Time.deltaTime * mSpeed;
            mRB.MovePosition(transform.position+(Vector3)tNextStep);
            yield return new WaitForFixedUpdate();
            tPath = vTarget.transform.position - transform.position;
        } while (true);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject == Target) {
            CancelTrack();
            Debug.LogFormat("{0} hit {1}",gameObject.name,coll.gameObject.name);
        }
    }
}
