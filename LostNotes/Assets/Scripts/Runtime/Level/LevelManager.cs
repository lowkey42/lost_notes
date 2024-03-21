using System.Collections;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace LostNotes.Level {
	internal sealed class LevelManager : MonoBehaviour {
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> winLevelChannelReference;
		private GameObjectEventChannel winLevelChannel;
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> loseLevelChannelReference;
		private GameObjectEventChannel loseLevelChannel;

		[SerializeField, Expandable]
		private LevelOrder _levels;

		private IEnumerator Start() {
			yield return winLevelChannelReference.LoadAssetAsync(asset => {
				winLevelChannel = asset;
				winLevelChannel.onTrigger += HandleWin;
			});

			yield return loseLevelChannelReference.LoadAssetAsync(asset => {
				loseLevelChannel = asset;
				loseLevelChannel.onTrigger += HandleLose;
			});
		}

		private void OnDestroy() {
			if (winLevelChannel) {
				winLevelChannel.onTrigger -= HandleWin;
				winLevelChannelReference.ReleaseAsset();
			}

			if (loseLevelChannel) {
				loseLevelChannel.onTrigger -= HandleLose;
				loseLevelChannelReference.ReleaseAsset();
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

		private void HandleLose(GameObject obj) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
