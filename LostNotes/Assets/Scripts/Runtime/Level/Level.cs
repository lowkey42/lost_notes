using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal sealed class Level : MonoBehaviour {
		[SerializeField] private Tilemap _interactableLayer;

		public InteractableTile GetTileAt(Vector2Int position) {
			return null;
		}
	}
}
