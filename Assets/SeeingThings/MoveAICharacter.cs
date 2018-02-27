using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAICharacter : FakePhysics {

    AITracking mAITracking;     //Link to tracker

    protected override void StartPhysics() {
        mAITracking = GetComponent<AITracking>();
    }

    protected override void UpdatePhysicsInput() {
        mCurrentMoveInput = Vector3.zero;
        if (mAITracking.TrackedObject!=null) {
            if(mAITracking.SeenObject!=null) {
                SetDebugText(string.Format("Seeing & Tracking {0:s}", mAITracking.TrackedObject.name), Color.green);
            } else {
                SetDebugText(string.Format("Tracking {0:s}\nLost {1:f2}", mAITracking.TrackedObject.name,mAITracking.mLostTimer), Color.red);
            }
            Vector3 tEnemyPosition = mAITracking.TrackedObject.transform.position;
            Vector3 tEnemyDirection = (tEnemyPosition - transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(tEnemyDirection);
            mCurrentMoveInput = tEnemyDirection * MoveSpeed;
        } else {
            SetDebugText("Bored", Color.gray);
            mCurrentMoveInput = Vector3.zero;
        }
    }

}
