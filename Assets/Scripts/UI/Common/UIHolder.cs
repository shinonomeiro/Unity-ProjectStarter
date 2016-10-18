using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSLib.UI
{
	/// <summary>
	/// Generic UI panel holder. Holds dictionary of cached UI elements.
	/// </summary>
	public sealed class UIHolder<T> where T : BaseUI
	{
		/// <summary>
		/// The UI dictionary.
		/// </summary>
		private Dictionary<string, T> _dictionary;

		/// <summary>
		/// Gets the dictionary.
		/// </summary>
		public Dictionary<string, T> dictionary
		{
			get	{ return _dictionary; }
		}	

		public UIHolder ()
		{
			_dictionary = new Dictionary<string, T>();
		}

		public T[] GetAll ()
		{
			T[] arr = new T[_dictionary.Count];
			_dictionary.Values.CopyTo (arr, 0);
			return arr;
		}

		public void Remove (T panel)
		{
			if (_dictionary.ContainsKey (panel.name))
			{
				GameObject.Destroy (_dictionary[panel.name].gameObject);
				_dictionary[panel.name] = null;
				_dictionary.Remove (panel.name);
			}
		}

		/// <summary>
		/// Cleans up non cached UI panels.
		/// </summary>
		public void CleanUpNonCached ()
		{
			CleanUp (panel => !panel.isCached);
		}

		public void CleanUpAll ()
		{
			CleanUp (panel => true);
		}
		
		/// <summary>
		/// Destroys all panels in the dictionary.
		/// </summary>
		void CleanUp (Predicate<BaseUI> p)
		{
			int count = _dictionary.Count;
			string[] keys = new string[count];
			_dictionary.Keys.CopyTo (keys, 0);

			for (int i=0; i<count; i++)
			{
				if ( p (_dictionary[keys[i]]) )
				{
					GameObject.Destroy (_dictionary[keys[i]]);
					_dictionary.Remove (keys[i]);
				}
			}
		}
	}
}