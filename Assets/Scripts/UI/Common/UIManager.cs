using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SSLib.UI
{
	public sealed class UIManager : MonoSingleton<UIManager>
	{
		private const string PATH = "Prefabs/UI/";
	
		// CANVASES
		
		[SerializeField]
		private Canvas _screenCanvas;
		
		[SerializeField]
		private Canvas _popupCanvas;
		
		// BACKGROUND
		
		[SerializeField]
		private Image _backgroundImage;

		// HOLDER

		private UIHolder<BaseUI> _panels;

		// //

		private List<BaseUI> _displayed;
		private int _topCount;

		// //
		
		// METHODS

		UIManager ()
		{
			_panels 	= new UIHolder<BaseUI> ();
			_displayed	= new List<BaseUI> ();
		}

		void Start ()
		{

		}

		public void SetBackgroundImage (Sprite newImage)
		{
			_backgroundImage.sprite = newImage;
		}

		public void ShowBackgroundImage (bool enabled)
		{
			_backgroundImage.gameObject.SetActive (enabled);
		}

		/// <summary>
		/// Retrieves a reference to a panel.
		/// Both ScreenUIs and PopupUIs are supported.
		/// </summary>
		public T GetPanel<T> (string name) where T : BaseUI
		{
			BaseUI result = null;

			if (_panels.dictionary.ContainsKey (name))
			{
				result = _panels.dictionary[name];
			}

			else
			{
				UnityEngine.Object panelObj = Resources.Load (PATH + name + "/" + name);

				if (panelObj == null)
				{
					Debug.LogError ("An error has occurred. The specified panel name may be incorrect.");
					return null;
				}

				GameObject panelGo = Instantiate (panelObj) as GameObject;
				BaseUI panel = panelGo.GetComponent<BaseUI>();
				_panels.dictionary[name] = panel;
				
				if (panel is ScreenUI) panel.transform.SetParent (_screenCanvas.transform, false);
				else if (panel is PopupUI) panel.transform.SetParent (_popupCanvas.transform, false);
				
				panelGo.transform.localPosition = Vector3.zero;
				panelGo.transform.localScale = Vector3.one;
				panelGo.name = name;
				
				panelGo.SetActive (false);

				result = panel;
			}

			return result as T;
		}

		// DISPLAY FUNCTIONS

		/// <summary>
		/// Displays the specified panel.
		/// Use this preferably when using coroutines.
		/// </summary>
		public BaseUI Display (BaseUI panel)
		{
			return Display (panel, null);
		}

		/// <summary>
		/// Displays the specified panel.
		/// Use when not using coroutines.
		/// </summary>
		public BaseUI Display (BaseUI panel, Action onComplete)
		{
			if (panel.gameObject.activeInHierarchy || panel.isFading)
			{
				Debug.LogWarning ("[Display] Invalid operation: " + panel.name + " is fading or already displayed.");
				return panel;
			}

			StartCoroutine (IEDisplayPanel (panel, onComplete));

			return panel;
		}

		/// <summary>
		/// Hides the specified panel.
		/// Use this preferably when using coroutines.
		/// </summary>
		public BaseUI Hide (BaseUI panel)
		{
			return Hide (panel, null);
		}

		/// <summary>
		/// Hides the specified panel.
		/// Use when not using coroutines.
		/// </summary>
		public BaseUI Hide (BaseUI panel, Action onComplete)
		{
			if (!panel.gameObject.activeInHierarchy || panel.isFading)
			{
				Debug.LogWarning ("[Hide] Invalid operation: " + panel.name + " is fading or already hidden.");
				return panel;
			}

			StartCoroutine (IEHidePanel (panel, onComplete));

			return panel;
		}

		/// <summary>
		/// Hides all currently displayed panels.
		/// Callback is invoked once the last panel has done fading.
		/// </summary>
		public void HideAll (Action onComplete)
		{	
			BaseUI[] panels = new BaseUI[_displayed.Count];

			for (int i=0; i<_displayed.Count; i++)
			{
				panels[i] = _displayed[i];
			}

			for (int i=0; i<panels.Length; i++)
			{
				if (panels[i].isFading)
				{
					Debug.LogWarning ("[HideAll] Invalid operation: " + panels[i].name + " is still fading.");
					return;
				}
			}

			int counter = 0;

			for (int i=0; i<panels.Length; i++)
			{
				Hide (panels[i], () => { if (++counter == panels.Length) onComplete (); });
			}
		}

		IEnumerator IEDisplayPanel (BaseUI panel, Action onComplete)
		{
			if (panel is ScreenUI)
			{
				ScreenUI screen = panel as ScreenUI;

				if (screen.alwaysOnTop)
				{
					panel.transform.SetAsLastSibling ();
					_topCount++;
				}

				else
				{
					screen.transform.SetSiblingIndex (_screenCanvas.transform.childCount - 1 - _topCount);
				}
			}

			panel.gameObject.SetActive (true);

			CanvasGroup cg = panel.GetComponent<CanvasGroup>();
			cg.interactable = false;

			_displayed.Add (panel);

			panel.SendMessage ("Display");

			yield return panel;

			cg.interactable = true;

			if (onComplete != null) onComplete ();
		}
		
		IEnumerator IEHidePanel (BaseUI panel, Action onComplete)
		{
			if (panel is ScreenUI && ((ScreenUI)panel).alwaysOnTop)
			{
				_topCount--;
			}

			CanvasGroup cg = panel.GetComponent<CanvasGroup>();
			cg.interactable = false;

			_displayed.Remove (panel);

			panel.SendMessage ("Hide");

			yield return panel;

			if (panel.isCached)
			{
				cg.interactable = true;
				panel.gameObject.SetActive (false);
			}
			else
			{
				_panels.Remove (panel);
			}

			if (onComplete != null) onComplete ();
		}
	}
}