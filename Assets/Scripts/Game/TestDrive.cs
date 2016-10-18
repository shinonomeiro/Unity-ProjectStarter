using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SSLib.UI;

public class TestDrive : ExtendedMonoBehaviour
{
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// PANELS

		if (Input.GetKeyUp (KeyCode.A))
		{
			Display ();
		}

		if (Input.GetKeyUp (KeyCode.S)) 
		{
			StartCoroutine (IEDisplay ());
		}

		if (Input.GetKeyUp (KeyCode.T)) 
		{
			HeaderScreen header = UIManager.Instance.GetPanel<HeaderScreen>(PanelList.HEADER_SCREEN);
			UIManager.Instance.Display (header, () => Debug.Log ("Header screen!"));

			FooterScreen footer = UIManager.Instance.GetPanel<FooterScreen>(PanelList.FOOTER_SCREEN);
			UIManager.Instance.Display (footer, () => Debug.Log ("Footer screen!"));
		}

		if (Input.GetKeyUp (KeyCode.P))
		{
			DisplayPopupChain ();
		}

		if (Input.GetKeyUp (KeyCode.J)) 
		{
			StartCoroutine (IEHide ());
		}

		if (Input.GetKeyUp (KeyCode.H)) 
		{
			UIManager.Instance.HideAll (() => SceneUtils.Instance.ChangeScene (SceneList.GAME, null, null, false));
		}

		// SCENES

		if (Input.GetKeyUp (KeyCode.G)) 
		{
			SceneUtils.Instance.ChangeScene (SceneList.GAME, null, null, false);
		}

		if (Input.GetKeyUp (KeyCode.M)) 
		{
			SceneUtils.Instance.ChangeScene (SceneList.MAIN, null, null, false);
		}
	}

	string[] messages = new string[] { "message_1", "message_2", "message_3", "message_4", "message_5" };
	int index = 0;

	void DisplayPopupChain ()
	{
		if (index == messages.Length) return;

		SimpleMessagePopup popup = UIManager.Instance.GetPanel<SimpleMessagePopup>(PanelList.SINGLE_BUTTON_POPUP);
		popup.parameters["header"] = "Header";
		popup.parameters["body"] = messages[index];
		popup.SetButton (0, null, () => UIManager.Instance.Hide (popup, DisplayPopupChain));
		UIManager.Instance.Display (popup);

		index++;
	}

	void Display ()
	{
		ExampleScreen screen = UIManager.Instance.GetPanel<ExampleScreen>(PanelList.EXAMPLE_SCREEN);

		UIManager.Instance.Display (screen, () => Debug.Log ("Screen display complete!"));

//		SimpleMessagePopup popup = UIManager.Instance.GetPanel<SimpleMessagePopup>(PanelList.SINGLE_BUTTON_POPUP);
//		popup.parameters["header"] = "Test Header";
//		popup.parameters["body"] = "Test Body";
//		popup.SetButton (0, null, () => UIManager.Instance.Hide (popup));
//
//		UIManager.Instance.Display (popup, () => Debug.Log ("Popup display complete!"));

		SimpleMessagePopup popup = UIManager.Instance.GetPanel<SimpleMessagePopup>(PanelList.DOUBLE_BUTTON_POPUP);
		popup.parameters["header"] = "Test Header";
		popup.parameters["body"] = "Test Body";
		popup.SetButton (0, null, () => UIManager.Instance.Hide (popup));
		popup.SetButton (1, null, () => UIManager.Instance.Hide (popup));

		UIManager.Instance.Display (popup, () => Debug.Log ("Popup display complete!"));
	}

	void Hide ()
	{
		ExampleScreen screen = UIManager.Instance.GetPanel<ExampleScreen>(PanelList.EXAMPLE_SCREEN);

		UIManager.Instance.Hide (screen, () => Debug.Log ("Screen hide complete!"));
	}

	IEnumerator IEDisplay ()
	{
		ExampleScreen screen = UIManager.Instance.GetPanel<ExampleScreen>(PanelList.EXAMPLE_SCREEN);

		yield return UIManager.Instance.Display (screen);

		SimpleMessagePopup popup = UIManager.Instance.GetPanel<SimpleMessagePopup>(PanelList.SINGLE_BUTTON_POPUP);
		popup.parameters["header"] = "Test Header";
		popup.parameters["body"] = "Test Body";
		popup.SetButton (0, null, () => UIManager.Instance.Hide (popup));

		yield return UIManager.Instance.Display (popup);

		Debug.Log ("Display complete!");
	}

	IEnumerator IEHide ()
	{
		ExampleScreen screen = UIManager.Instance.GetPanel<ExampleScreen>(PanelList.EXAMPLE_SCREEN);

		yield return UIManager.Instance.Hide (screen);

		SimpleMessagePopup popup = UIManager.Instance.GetPanel<SimpleMessagePopup>(PanelList.SINGLE_BUTTON_POPUP);

		yield return UIManager.Instance.Hide (popup);

		Debug.Log ("Hide complete!");
	}
}
