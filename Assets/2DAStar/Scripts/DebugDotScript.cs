using UnityEngine;
using System.Collections;

public class DebugDotScript : MonoBehaviour {

	public	float	DotDuration=1.0f;

	public	Color	DotColour=Color.green;

	private	SpriteRenderer	mSprite;


	void Start () {
		IEnumerator	DoFade=Fade(DotDuration);
		mSprite = GetComponent<SpriteRenderer> ();
		mSprite.color = DotColour;
		StartCoroutine (DoFade);
	}

	IEnumerator	Fade(float vTime) {
		float	tTimeOut = 0.0f;
		Vector4	tTarget = DotColour;
		Vector2	tScale = Vector2.zero;
		tTarget.w = 0.0f;

		while (tTimeOut <=vTime) {
			mSprite.color = Vector4.Lerp (DotColour, tTarget, tTimeOut / vTime);
			mSprite.transform.localScale = Vector2.Lerp (Vector2.one, tScale, tTimeOut / vTime);
			tTimeOut += Time.deltaTime;
			yield	return	null;
		}
		Destroy (this.gameObject);
	}

}
