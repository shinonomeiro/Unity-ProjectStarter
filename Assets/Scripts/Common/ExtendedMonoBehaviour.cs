using UnityEngine;
using System.Collections;

public class ExtendedMonoBehaviour : MonoBehaviour
{
	private GameObject _gameObject;
	
	public new GameObject gameObject
	{
		get
		{
			if (_gameObject == null)
			{
				_gameObject = base.gameObject;
			}
			
			return _gameObject;
		}
	}

	private Transform _transform;

	public new Transform transform
	{
		get
		{
			if (_transform == null)
			{
				_transform = base.transform;
			}

			return _transform;
		}
	}

	// // //

	// ADD OTHER USEFUL METHODS / PROPERTIES HERE
}
