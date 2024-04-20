using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Emericoude.SceneManagement;

namespace Emericoude.Gameplay
{
	public static class GameObjectExtensions
	{
		/// <summary> Instantiates a gameobject in the target scene. </summary>
		/// <param name="sceneName"> The scene by string name. </param>
		/// <param name="go"> The object to spawn. If <see langword="null"/>, a new gameObject is created. </param>
		/// <param name="position"> The position at which to spawn the object. If <see langword="null"/>, Vector.zero. </param>
		/// <param name="rotation"> The rotation at which to spawn the object. If <see langword="null"/>, Quaternion.identity. </param>
		/// <param name="parent"> The parent of the object, note that this can override the scene target. The value can be null. </param>
		/// <param name="forceSceneLoad"> USE WITH CAUTION. Forces the target scene to be created or loaded if it is not loaded. Note that this uses <seealso cref="SceneManagerExtensions.GetOrLoadSceneAdditively(string)"/>, which is not an <see langword="async"/> load.</param>
		/// <returns> The game object that was instantiated. </returns>
		public static GameObject InstantiateInScene (string sceneName, GameObject go = null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, bool forceSceneLoad = false)
		{
			return InstantiateInScene(SceneManager.GetSceneByName(sceneName), go, position, rotation, parent, forceSceneLoad);
		}

		/// <summary> Instantiates a gameobject in the target scene. </summary>
		/// <param name="scene"> The target scene. </param>
		/// <param name="go"> The object to spawn. If <see langword="null"/>, a new gameObject is created. </param>
		/// <param name="position"> The position at which to spawn the object. If <see langword="null"/>, Vector.zero. </param>
		/// <param name="rotation"> The rotation at which to spawn the object. If <see langword="null"/>, Quaternion.identity. </param>
		/// <param name="parent"> The parent of the object, note that this can override the scene target. The value can be null. </param>
		/// <param name="forceSceneLoad"> USE WITH CAUTION. Forces the target scene to be created or loaded if it is not loaded. Note that this uses <seealso cref="SceneManagerExtensions.GetOrLoadSceneAdditively(string)"/>, which is not an <see langword="async"/> load.</param>
		/// <returns> The game object that was instantiated. </returns>
		public static GameObject InstantiateInScene (Scene scene, GameObject go = null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, bool forceSceneLoad = false)
		{
			if (forceSceneLoad)
			{
				SceneManager.SetActiveScene(SceneManagerExtensions.GetOrLoadSceneAdditively(scene));
			}
			else
			{
				if (scene.isLoaded && scene.IsValid())
				{
					SceneManager.SetActiveScene(scene);
				}
				else
				{
					Debug.LogWarning($"Scene {scene.name} is invalid ({scene.IsValid()}) or not loaded ({scene.isLoaded}). Spawning in {SceneManager.GetActiveScene().name} instead.");
				}
			}

			if (go == null)
			{
				return new GameObject();
			}

			return GameObject.Instantiate(go, position, rotation, parent);
		}
	}
}
