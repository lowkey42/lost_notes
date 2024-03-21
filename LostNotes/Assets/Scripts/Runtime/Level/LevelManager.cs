using System.Collections;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace LostNotes.Level {
	internal sealed class LevelManager : MonoBehaviour {
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _winLevelChannelReference;
		private GameObjectEventChannel _winLevelChannel;
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _loseLevelChannelReference;
		private GameObjectEventChannel _loseLevelChannel;

		[SerializeField, Expandable]
		private LevelOrder _levels;

		private IEnumerator Start() {
			yield return _winLevelChannelReference.LoadAssetAsync(asset => {
				_winLevelChannel = asset;
				_winLevelChannel.onTrigger += HandleWin;
			});

			yield return _loseLevelChannelReference.LoadAssetAsync(asset => {
				_loseLevelChannel = asset;
				_loseLevelChannel.onTrigger += HandleLose;
			});
		}

		private void OnDestroy() {
			if (_winLevelChannel) {
				_winLevelChannel.onTrigger -= HandleWin;
				_winLevelChannelReference.ReleaseAsset();
			}

			if (_loseLevelChannel) {
				_loseLevelChannel.onTrigger -= HandleLose;
				_loseLevelChannelReference.ReleaseAsset();
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
