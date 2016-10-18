using UnityEngine;
using System.Collections;
using DG.Tweening;
using SSLib.UI;

public class SlideFader : Fader 
{
	public float fadeSpeed = 1;

	protected override IEnumerator IEFade (FadeState state)
	{
		RectTransform rt = GetComponent<RectTransform>();
		rt.localPosition = state == FadeState.Display ? new Vector3 (Screen.width, 0, 0) : Vector3.zero ;
		Vector3 targetPos = state == FadeState.Display ? Vector3.zero : new Vector3 (-Screen.width, 0, 0);

		isFading = true;

		yield return rt.DOLocalMove (targetPos, fadeSpeed).SetEase (Ease.Linear).WaitForCompletion ();

		isFading = false;
	}
}
