using System.Collections;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Gameplay {
	internal sealed class PingChannelOnEnter : MonoBehaviour, ICollisionMessages {
		[SerializeField, Tag]
		private string actorTag = "Player";

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _channelReference;
		private GameObjectEventChannel _channel;

		private IEnumerator Start() {
			yield return _channelReference.LoadAssetAsync(asset => _channel = asset);
		}

		private void OnDestroy() {
			if (_channel) {
				_channelReference.ReleaseAsset();
			}
		}

		public void OnActorEnter(GameObject actor) {
			if (!_channel) {
				return;
			}

			if (actor.CompareTag(actorTag)) {
				_channel.Raise(gameObject);
			}
		}
	}
}
