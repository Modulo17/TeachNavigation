using UnityEngine;
using System.Collections;

public	abstract class SpriteHelper : MonoBehaviour {

	protected	Animator 		mAnimator;
	protected	SpriteRenderer 	mSpriteRenderer;

	protected	delegate	void	ArrivedAt();

	protected virtual void	Awake() {
		mAnimator = GetComponent<Animator>();
		mSpriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected virtual	IEnumerator MoveTo(Vector3 vNewPosition, float vSpeed, ArrivedAt vCallback=null) {
		float	tStart = Time.fixedTime;
		float	tD = 0.0f;
		Vector3	tFrom = transform.position;
		Vector3	tDirection = vNewPosition - transform.position;
		mSpriteRenderer.flipX=(tDirection.x>=0.0f);
		float	tDistance = Vector3.Magnitude (tDirection);
		float	tTime = 1.0f / vSpeed;
		tTime *= tDistance;
		mAnimator.SetBool("Walking", true);
		mAnimator.SetFloat("Speed", vSpeed);
		while(tDistance>Mathf.Epsilon) {
			transform.position = Vector3.Lerp (tFrom, vNewPosition, tD/tTime);
			tDistance = Vector3.Magnitude (vNewPosition - transform.position);
			tD += Time.deltaTime;
			yield	return	null;
		}
		transform.position = vNewPosition;
		mAnimator.SetBool("Walking", false);
		if (vCallback != null)
			vCallback ();
	}
}
