using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Level {
	internal sealed class LevelManager : MonoBehaviour {
		[SerializeField, Expandable]
		private GameObjectEventChannel winLevelChannel;
		[SerializeField, Expandable]
		private LevelOrder _levels;

		private void OnEnable() {
			winLevelChannel.onTrigger += HandleWin;
		}
		private void OnDisable() {
			winLevelChannel.onTrigger -= HandleWin;
		}

		private void HandleWin(GameObject obj) {
			if (_levels.TryGetLevelIndex(obj.scene, out var index)) {
				if (_levels.TryGetLevelScene(index + 1, out var scene)) {
					scene.LoadScene();
				} else {
					Debug.Log("YOU WIN THE GAME");
				}
			}
		}
	}
}
