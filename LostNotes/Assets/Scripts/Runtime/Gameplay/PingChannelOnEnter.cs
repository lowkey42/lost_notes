using System.Collections;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Gameplay {
	internal sealed class PingChannelOnEnter : MonoBehaviour, ICollisionMessages {
		[SerializeField, Tag]
		private string actorTag = "Player";

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> channelReference;
		private GameObjectEventChannel channel;

		private IEnumerator Start() {
			yield return channelReference.LoadAssetAsync(asset => channel = asset);
		}

		private void OnDestroy() {
			if (channel) {
				channelReference.ReleaseAsset();
			}
		}

		public void OnActorEnter(GameObject actor) {
			if (!channel) {
				return;
			}

			if (actor.CompareTag(actorTag)) {
				channel.Raise(gameObject);
			}
		}
	}
}
