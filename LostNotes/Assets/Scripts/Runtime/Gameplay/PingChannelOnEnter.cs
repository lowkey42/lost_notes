using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class PingChannelOnEnter : MonoBehaviour, ICollisionMessages {
		[SerializeField, Tag]
		private string actorTag = "Player";

		[SerializeField, Expandable]
		private GameObjectEventChannel channel;

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
