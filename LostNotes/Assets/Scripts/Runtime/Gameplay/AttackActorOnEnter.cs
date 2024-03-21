using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class AttackActorOnEnter : MonoBehaviour {
		[SerializeField]
		private bool _disableActor = false;
		[SerializeField]
		private bool _destroyActor = false;

		public void OnActorEnter(GameObject actor) {
			actor.BroadcastMessage(nameof(IAttackMessages.OnAttacked), null, SendMessageOptions.DontRequireReceiver);
			if (_disableActor) {
				actor.SetActive(false);
			}

			if (_destroyActor) {
				Destroy(actor);
			}
		}
	}
}
