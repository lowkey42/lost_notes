using System.Collections;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Level {
	internal sealed class LevelManager : MonoBehaviour {
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> winLevelChannelReference;
		private GameObjectEventChannel winLevelChannel;
		[SerializeField, Expandable]
		private LevelOrder _levels;

		private IEnumerator Start() {
			var handle = winLevelChannelReference.LoadAssetAsync();
			yield return handle;
			winLevelChannel = handle.Result;
			winLevelChannel.onTrigger += HandleWin;
		}

		private void OnDisable() {
			if (winLevelChannel) {
				winLevelChannel.onTrigger -= HandleWin;
			}
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
