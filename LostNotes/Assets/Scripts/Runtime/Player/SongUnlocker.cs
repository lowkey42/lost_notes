using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Player {
	internal sealed class SongUnlocker : MonoBehaviour {

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _pageSpawnChannelReference;
		private GameObjectEventChannel _pageSpawnChannel;

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _pageCollectChannelReference;
		private GameObjectEventChannel _pageCollectChannel;

		private IEnumerator Start() {
			yield return _pageSpawnChannelReference.LoadAssetAsync(asset => {
				_pageSpawnChannel = asset;
				_pageSpawnChannel.onTrigger += HandleSongSpawn;
			});

			yield return _pageCollectChannelReference.LoadAssetAsync(asset => {
				_pageCollectChannel = asset;
				_pageCollectChannel.onTrigger += HandleSongCollect;
			});
		}

		private void OnDestroy() {
			if (_pageSpawnChannel) {
				_pageSpawnChannelReference.ReleaseAsset();
			}

			if (_pageCollectChannel) {
				_pageCollectChannelReference.ReleaseAsset();
			}
		}

		private readonly Dictionary<SongAsset, int> unlocking = new();

		private void HandleSongSpawn(GameObject obj) {
			if (obj.TryGetComponent<SongProvider>(out var provider)) {
				unlocking[provider.Song] = unlocking.GetValueOrDefault(provider.Song) + 1;
			}
		}

		private void HandleSongCollect(GameObject obj) {
			if (obj.TryGetComponent<SongProvider>(out var provider)) {
				unlocking[provider.Song]--;

				if (unlocking[provider.Song] == 0) {
					provider.Song.IsAvailable = true;
				}

				Destroy(obj);
			}
		}
	}
}
