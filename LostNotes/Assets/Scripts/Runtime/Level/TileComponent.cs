using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Level {
	internal sealed class TileComponent : MonoBehaviour, ITileMeta, IActorStatusMessages {
		[SerializeField]
		private bool _isWalkable = false;

		public bool IsWalkable => _isWalkable || _sleeping;

		[SerializeField]
		private bool _isInteractionBlocking = false;

		public bool IsInteractionBlocking => _isInteractionBlocking;

		private bool _sleeping = false;

		public void OnStatusEffectsChanged(StatusEffects statusEffects) {
			_sleeping = statusEffects.HasFlag(StatusEffects.Sleeping);
		}
	}
}
