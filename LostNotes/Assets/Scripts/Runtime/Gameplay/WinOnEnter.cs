using MyBox;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class WinOnEnter : MonoBehaviour, ICollisionMessages {
		[SerializeField, Tag]
		private string actorTag = "Player";

		public void OnActorEnter(GameObject actor) {
			if (actor.CompareTag(actorTag)) {
				Debug.Log("A winner is you!");
			}
		}
	}
}
