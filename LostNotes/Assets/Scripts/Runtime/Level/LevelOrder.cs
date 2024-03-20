using System;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostNotes.Level {
	[CreateAssetMenu]
	internal sealed class LevelOrder : ScriptableObject {
		[SerializeField]
		private SceneReference[] _levelScenes = Array.Empty<SceneReference>();

		public bool TryGetLevelScene(int index, out SceneReference scene) {
			scene = index < _levelScenes.Length
				? _levelScenes[index]
				: null;
			return scene is not null;
		}

		public bool TryGetLevelIndex(Scene scene, out int index) {
			for (var i = 0; i < _levelScenes.Length; i++) {
				if (_levelScenes[i].SceneName == scene.name) {
					index = i;
					return true;
				}
			}

			index = default;
			return false;
		}
	}
}
