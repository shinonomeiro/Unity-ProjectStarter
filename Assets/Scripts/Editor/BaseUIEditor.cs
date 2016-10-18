using UnityEngine;
using System.IO;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using SSLib.UI;

[CustomEditor(typeof(BaseUI), true)]
public class BaseUIEditor : Editor 
{
	public const string PATH_SCENES 	= "Assets/Scenes/UI";
	public const string PATH_PREFABS 	= "Assets/Resources/Prefabs/UI";

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("Update prefab"))
		{
			UpdatePrefab ();
		}

		DrawDefaultInspector ();
	}

	void UpdatePrefab ()
	{
		Debug.Log ("Updating prefab...");

		string sceneName 			= EditorSceneManager.GetActiveScene ().name;
		string sceneFolderName		= new DirectoryInfo (EditorSceneManager.GetActiveScene ().path).Parent.Name;

		string prefabFolderPath 	= Path.Combine (PATH_PREFABS, sceneName);
		string prefabPath 			= Path.Combine (prefabFolderPath, sceneName + ".prefab");

		// Check if the name of the scene is valid. It must contain either "Screen" or "Popup"

		if (!sceneName.Contains ("Screen") && !sceneName.Contains ("Popup"))
		{
			Debug.LogError ("The name of the scene must contain either 'Screen' or 'Popup'.");

			return;
		}

		// Check if the name of the panel matches the scene's

		if (target.name != sceneName)
		{
			Debug.LogError ("The name of this panel does not match the scene's. Please correct.");

			return;
		}

		// Check if scene's name matches its container folder's

		if (sceneFolderName != sceneName)
		{
			Debug.LogError ("The name of the folder does not match with the scene's. Please check, e.g. MyScreen/MyScreen.unity.");

			return;
		}

		// Check if prefab folder exists

		if (!Directory.Exists (prefabFolderPath))
		{
			Directory.CreateDirectory (prefabFolderPath);
		}

		// Check if prefab (empty or not) exists

		if (!File.Exists (prefabPath))
		{
			PrefabUtility.CreateEmptyPrefab (prefabPath);

			Debug.Log (sceneName + " prefab does not exist or has been deleted. Generating a new one...");
		}

		// Update the prefab with the current object
		// The panel will remain as a non prefab in the scene, which is exactly what we want to achieve

		PrefabUtility.CreatePrefab (prefabPath, ((BaseUI)target).gameObject, ReplacePrefabOptions.ReplaceNameBased);
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();

		Debug.Log ("Updated " + target.name + " (" + prefabPath + ")");
	}
}
