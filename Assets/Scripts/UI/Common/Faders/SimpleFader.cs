using UnityEngine;
using System.Collections;
using DG.Tweening;
using SSLib.UI;

public class SimpleFader : Fader 
{
	public float fadeSpeed = 1;

	protected override IEnumerator IEFade (FadeState state)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = state == FadeState.Display ? 0 : 1;
		float targetVal = Mathf.Abs (cg.alpha - 1);

		isFading = true;

		yield return cg.DOFade (targetVal, fadeSpeed).SetEase (Ease.Linear).WaitForCompletion ();

		isFading = false;
	}
}
