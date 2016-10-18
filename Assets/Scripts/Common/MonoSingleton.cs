﻿using UnityEngine;

/// <summary>
/// Generic Mono singleton.
/// </summary>
public abstract class MonoSingleton<T> : ExtendedMonoBehaviour where T : MonoSingleton<T>
{	
	private static T m_Instance = null;
	
	public static T Instance
	{
		get
		{
			if ( m_Instance == null )
			{
				m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

				if ( m_Instance == null )
				{
					m_Instance = new GameObject ("[" + typeof(T).ToString() + "]", typeof(T)).GetComponent<T>();
					m_Instance.Init ();
				}
				
			}

			return m_Instance;
		}
	}
	
	private void Awake ()
	{	
		if ( m_Instance == null )
		{
			m_Instance = this as T;
		}
	}
	
	public virtual void Init () {}
	
	private void OnApplicationQuit () 
	{
		m_Instance = null;
	}
}