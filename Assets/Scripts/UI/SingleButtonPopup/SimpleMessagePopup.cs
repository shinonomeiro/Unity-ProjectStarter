using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SSLib.UI;

/// <summary>
/// Provided as SAMPLE for a simple popup with:
/// - A header (key: header)
/// - A body (key: body)
/// - An arbitrary number of buttons
/// </summary>
public sealed class SimpleMessagePopup : PopupUI
{
	[SerializeField]
	private Text _header;
	[SerializeField]
	private Text _body;
	[SerializeField]
	private Button[] _buttons;

	protected override void OnDisplayStart ()
	{
		base.OnDisplayStart ();

		_header.text = parameters["header"] as string;
		_body.text = parameters["body"] as string;
	}

	/// <summary>
	/// Configures the button at the specified index in the array. 
	/// Set 'text' to null if you want to keep the current string.
	/// Note that this function only supports static buttons, i.e. 
	/// buttons set from the editor at compile-time.
	/// </summary>
	public void SetButton (int index, string text, UnityEngine.Events.UnityAction onClick)
	{
		if (text != null)
		{
			_buttons[index].GetComponentInChildren<Text>().text = text;
		}

		_buttons[index].onClick.RemoveAllListeners ();
		_buttons[index].onClick.AddListener (onClick);
	}
}