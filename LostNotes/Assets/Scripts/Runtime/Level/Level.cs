using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal sealed class Level : MonoBehaviour {
		[SerializeField] private Tilemap _interactableLayer;
		[SerializeField] private BoundsInt _bounds = new(-6, -10, -6, 12, 20, 12);

		public InteractableTile GetTileAt(Vector2Int position) {
			return _interactableLayer.GetTile<InteractableTile>(position.SwizzleXZ());
		}

		public T GetInteractableAt<T>(Vector2Int position) where T : class {
			return null;
		}

		public bool IsWalkable(Vector2Int position) {
			var position3d = position.SwizzleXZ();
			if (!_bounds.Contains(position3d))
				return false;

			var tile = _interactableLayer.GetTile(position3d);
			return !tile || tile is not InteractableTile it || it.IsWalkable();
		}
	}
}
