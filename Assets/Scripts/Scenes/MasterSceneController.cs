using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SSLib.UI
{
	public class MasterSceneController : MonoBehaviour
	{
		[SerializeField]
		private string targetSceneName;

		void Start () 
		{
			SceneUtils.Instance.Init ();
			SceneUtils.Instance.ChangeScene (targetSceneName, null, null, false);
		}
	}
}