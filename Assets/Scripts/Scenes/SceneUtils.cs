using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SSLib.UI
{
	public class SceneUtils : MonoSingleton<SceneUtils>
	{
		/// <summary>
		/// All scenes that have been loaded so far.
		/// </summary>
		public List<Scene> 	cachedScenes 	{ get; private set; }

		/// <summary>
		/// The current active scene.
		/// </summary>
		public Scene 		currentScene 	{ get; private set; }

		/// <summary>
		/// Initialization
		/// </summary>
		public void Init ()
		{
			cachedScenes = new List<Scene>();
			currentScene = SceneManager.GetActiveScene ();
		}

		/// <summary>
		/// Changes the scene to the specified one.
		/// Do not forget to add the target scene to the Build settings panel.
		/// Trying to load the same scene as current one will do nothing.
		/// </summary>
		/// <param name="sceneName">The name of the scene.</param>
		/// <param name="callback">Callback invoked once load completes.</param>
		/// <param name="progress">Callback giving current loading progress.</param>
		/// <param name="destroyCurrent">Destroys the current scene from hierarchy and clears the cache. 
		/// If set to false, only its root gameobject will be disabled.</param>
		public void ChangeScene (string sceneName, Action callback, Action<float> progress, bool destroyCurrent)
		{
			if (destroyCurrent)
			{
				cachedScenes.Remove (currentScene);
				SceneManager.UnloadScene (currentScene);
			}
			else
			{
				currentScene.GetRootGameObjects()[0].SetActive (false);
			}

			Scene nextScene = new Scene ();

			for (int i=0; i<cachedScenes.Count; i++)
			{
				if (cachedScenes[i].name == sceneName)
				{
					nextScene = cachedScenes[i];
					break;
				}
			}

			if (string.IsNullOrEmpty (nextScene.name))
			{
				StartCoroutine (IEChangeScene (sceneName, callback, progress));
			}
			else
			{
				nextScene.GetRootGameObjects()[0].SetActive (true);
				SceneManager.SetActiveScene (nextScene);
				currentScene = nextScene;

				if (callback != null) callback ();
			}
		}

		IEnumerator IEChangeScene (string sceneName, Action callback, Action<float> progress)
		{
			AsyncOperation asyncOp = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);

			while (!asyncOp.isDone)
			{
				if (progress != null) progress (asyncOp.progress);
				yield return null;
			}

			Scene loadedScene = SceneManager.GetSceneByName (sceneName);
			cachedScenes.Add (loadedScene);
			loadedScene.GetRootGameObjects()[0].SetActive (true);
			SceneManager.SetActiveScene (loadedScene);
			currentScene = loadedScene;

			if (callback != null) callback ();
		}
	}
}
