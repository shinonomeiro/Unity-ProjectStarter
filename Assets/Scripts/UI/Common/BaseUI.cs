using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SSLib.UI
{
	public abstract class BaseUI : ExtendedMonoBehaviour, IEnumerator
	{
		// PARAMETERS

		private Dictionary<string, object> _parameters;

		/// <summary>
		/// Key-value store used to pass arguments to this panel.
		/// Beware of type safety as all values are passed as object.
		/// </summary>
		public Dictionary<string, object> parameters
		{
			get 
			{
				if (_parameters == null)
				{
					_parameters = new Dictionary<string, object>();
				}

				return _parameters;
			}
		}

		/// <summary>
		/// Determines if this panel is to be discarded upon hiding.
		/// </summary>
		public bool	isCached = true;

		// FADING

		[SerializeField]
		private Fader fader;

		/// <summary>
		/// Is this panel currently fading in/out?
		/// </summary>
		public bool isFading { get; private set; }

		// // //

		// CALLBACKS

		/// <summary>
		/// Override this to implement custom fade in logic.
		/// Generally a good place to (re-)initialize your data.
		/// </summary>
		protected virtual void OnDisplayStart ()
		{
			Debug.Log (name + ": Display start.");
		}

		/// <summary>
		/// Override this to implement custom logic to be executed after fading ends.
		/// </summary>
		protected virtual void OnDisplayEnd ()
		{
			Debug.Log (name + ": Display complete.");
		}

		/// <summary>
		/// Override this to implement custom fade out logic.
		/// </summary>
		protected virtual void OnHideStart ()
		{
			Debug.Log (name + ": Hide start.");
		}

		/// <summary>
		/// Override this to implement custom logic to be executed after fading ends.
		/// Generally a good place to release memory.
		/// </summary>
		protected virtual void OnHideEnd ()
		{
			Debug.Log (name + ": Hide complete.");
		}

		// // //

		IEnumerator IEFade (FadeState state)
		{
			isFading = true;

			Invoke ("On" + state + "Start", 0);

			if (fader != null)
			{
				fader.DoFade (state);
				yield return fader;
			}
			else
			{
				GetComponent<CanvasGroup>().alpha = state == FadeState.Display ? 1 : 0;
				yield return null;
			}

			Invoke ("On" + state + "End", 0);

			isFading = false;
		}

		// // //

		/// <summary>
		/// Displays this panel. Kept private and called via reflection by UIManager.
		/// </summary>
		void Display ()
		{
			StartCoroutine (IEFade (FadeState.Display));
		}

		/// <summary>
		/// Hides this panel. Kept private and called via reflection by UIManager.
		/// </summary>
		void Hide ()
		{
			StartCoroutine (IEFade (FadeState.Hide));
		}

		// // //

		#region IEnumerator implementation

		public bool MoveNext ()
		{
			return isFading;
		}

		public void Reset ()
		{
			return;
		}

		public object Current 
		{
			get 
			{
				return isFading;
			}
		}

		#endregion

		protected virtual void OnDestroy ()
		{
			if (_parameters != null)
			{
				_parameters.Clear ();
				_parameters = null;
			}
		}
	}

}