using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAICharacter : FakePhysics {

    AITracking mAITracking;     //Link to tracker

    Vector3 mStartingPos;
    Quaternion mStartingRotation;

    protected override void StartPhysics() {
        mAITracking = GetComponent<AITracking>();
        mStartingPos = transform.position;
        mStartingRotation = transform.rotation;
    }

    protected override void UpdatePhysicsInput() {
        SetDebugText(mAITracking.ToString(),Color.green);
        if (mAITracking.TrackedObject!=null) {
            Vector3 tEnemyPosition = mAITracking.TrackedObject.transform.position;
            Vector3 tEnemyDirection = (tEnemyPosition - transform.position).normalized;
            tEnemyDirection.y = 0.0f;   //Keep it on the ground

            transform.rotation = Quaternion.LookRotation(tEnemyDirection);
            mVelocity.x += tEnemyDirection.x * MoveSpeed;
            mVelocity.z += tEnemyDirection.z * MoveSpeed;
        } else {
            Vector3 tTargetDirection = (mStartingPos - transform.position); // Go back to start position
            tTargetDirection.y = 0.0f;
            float tDistance = tTargetDirection.magnitude;
            if(tDistance>Mathf.Epsilon) {
                tTargetDirection=tTargetDirection.normalized;
                transform.rotation = Quaternion.LookRotation(tTargetDirection);
                mVelocity.x += tTargetDirection.x * Mathf.Min(MoveSpeed,tDistance);
                mVelocity.z += tTargetDirection.z * Mathf.Min(MoveSpeed, tDistance);
            }
        }
    }
}
