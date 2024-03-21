using UnityEngine;

namespace LostNotes.Level {
	internal sealed class TileComponent : MonoBehaviour, ITileMeta {
		[SerializeField]
		private bool _isWalkable = false;
		public bool IsWalkable => _isWalkable;

		[SerializeField]
		private bool _isInteractionBlocking = false;

		public bool IsInteractionBlocking => _isInteractionBlocking;
	}
}
