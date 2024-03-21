using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Gameplay {
	internal sealed class PingChannelOnSpawn : MonoBehaviour {
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _channelReference;
		private GameObjectEventChannel _channel;

		private IEnumerator Start() {
			yield return _channelReference.LoadAssetAsync(asset => _channel = asset);
			yield return null;
			_channel.Raise(gameObject);
		}

		private void OnDestroy() {
			if (_channel) {
			}
		}
	}
}
