using UnityEngine;
using System.Collections;
using SSLib.UI;

public class AnimatorFader : Fader 
{
	[SerializeField]
	private Animator _animator;

	public float fadeSpeed = 1;

	void Awake ()
	{
		_animator.enabled = false;
		_animator.speed = fadeSpeed;
	}

	protected override IEnumerator IEFade (FadeState state)
	{	
		isFading = true;
		_animator.enabled = true;
		
		_animator.Play (state.ToString ());

		yield return null;
		
		float length = _animator.GetCurrentAnimatorStateInfo (0).length / fadeSpeed;
		float i = 0;
		
		while (i < length)
		{
			i += Time.deltaTime;
			yield return null;
		}

		_animator.enabled = false;
		isFading = false;
	}
}
