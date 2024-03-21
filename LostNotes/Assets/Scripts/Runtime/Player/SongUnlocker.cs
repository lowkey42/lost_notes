using System.Collections;
using System.Linq;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Player {
	internal sealed class SongUnlocker : MonoBehaviour {
		[SerializeField, Tag]
		private string _pageLabel = "Page";

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _pageCollectChannelReference;
		private GameObjectEventChannel _pageCollectChannel;

		private IEnumerator Start() {
			yield return _pageCollectChannelReference.LoadAssetAsync(asset => {
				_pageCollectChannel = asset;
				_pageCollectChannel.OnTrigger += HandleSongCollect;
			});
		}

		private void OnDestroy() {
			if (_pageCollectChannel) {
				_pageCollectChannelReference.ReleaseAsset();
			}
		}

		private void HandleSongCollect(GameObject obj) {
			if (obj.TryGetComponent<SongProvider>(out var provider)) {
				if (GameObject.FindGameObjectsWithTag(_pageLabel).Count(o => obj.TryGetComponent<SongProvider>(out var p) && p.Song == provider.Song) == 1) {
					provider.Song.IsAvailable = true;
					provider.Song.NotesLearned = 0;
				}

				Destroy(obj);
			}
		}
	}
}
