using UnityEngine;
using System.Collections;

namespace SSLib.UI
{
	/// <summary>
	/// The target state to achieve. 
	/// </summary>
	public enum FadeState
	{
		Display,
		Hide
	}

	/// <summary>
	/// Base class for all faders.
	/// </summary>
	public abstract class Fader : MonoBehaviour, IEnumerator
	{
		protected bool isFading;

		/// <summary>
		/// Starts fading to the target state.
		/// The are only two states: 'Display' and 'Hide'.
		/// </summary>
		public void DoFade (FadeState state)
		{
			StartCoroutine (IEFade (state));
		}

		protected abstract IEnumerator IEFade (FadeState state);

		#region IEnumerator implementation

		public virtual object Current
		{
			get 
			{ 
				return isFading;
			}
		}

		public virtual bool MoveNext ()
		{
			return isFading;
		}

		public void Reset ()
		{
			return;
		}

		#endregion
	}
}