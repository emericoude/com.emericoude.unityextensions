using UnityEngine.SceneManagement;

namespace Emericoude.Helpers
{
	public static class SceneManagementHelpers
	{
		/// <summary> Tries to get the scene from the build index. If not found, it creates it. <br/>
		/// If found, it checks if it is loaded, if it is not, it loads it first. </summary>
		/// <remarks> This load is NOT async, do not use this for expensive scene loads. </remarks>
		/// <returns> The requested scene. </returns>
		public static Scene GetOrLoadSceneAdditively (string name)
		{
			return GetOrLoadSceneAdditively(SceneManager.GetSceneByName(name));
		}

		/// <summary> Tries to get the scene from the build index. If not found, it creates it. <br/>
		/// If found, it checks if it is loaded, if it is not, it loads it first. </summary>
		/// <remarks> This load is NOT async, do not use this for expensive scene loads. </remarks>
		/// <returns> The requested scene. </returns>
		public static Scene GetOrLoadSceneAdditively (Scene scene)
		{
			if (!scene.IsValid())
			{
				return SceneManager.CreateScene(scene.name);
			}

			if (!scene.isLoaded)
			{
				SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
			}

			return scene;
		}
	}
}
