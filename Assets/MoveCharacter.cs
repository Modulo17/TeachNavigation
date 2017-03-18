using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RL_Helpers;
using UnityEngine.Analytics;

namespace SinglePlayer
{
	[RequireComponent(typeof(CharacterController))]

	public class MoveCharacter : MonoBehaviour {

		CharacterController	mCC;

		// Use this for initialization
		void Start () {
			mCC = GetComponent<CharacterController> ();
		}
		
		// Update is called once per frame
		void Update () {
			Move ();
		}

		float mJumpHeight = 10f;
		Vector3 mMoveDirection = Vector3.zero;
		public float MoveSpeed = 10f;

		void Move() {          //Move Character with controller
			if (mCC.isGrounded) {
				transform.Rotate(0, IC.GetInput(IC.Directions.MoveX)*360f*Time.deltaTime, 0);
				mMoveDirection.x = 0f;
				mMoveDirection.y = 0f;
				mMoveDirection.z = IC.GetInput(IC.Directions.MoveY);
				mMoveDirection = transform.TransformDirection(mMoveDirection);      //Move in direction character is facing
				mMoveDirection *= MoveSpeed;
				if (IC.GetInput(IC.Directions.Jump) > 0f) {
					mMoveDirection.y = mJumpHeight;        //Jump
				}
			}
			mMoveDirection.y += Physics.gravity.y * Time.deltaTime;
			mCC.Move(mMoveDirection * Time.deltaTime);
		}

		void OnDestroy() {
			GameOver ();
		}

		void	GameOver() {
		}
	}
}
