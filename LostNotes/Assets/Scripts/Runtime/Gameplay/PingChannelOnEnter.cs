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
			var handle = channelReference.LoadAssetAsync();
			yield return handle;
			channel = handle.Result;
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
