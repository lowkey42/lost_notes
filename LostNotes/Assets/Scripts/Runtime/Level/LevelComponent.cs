using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal sealed class LevelComponent : MonoBehaviour {
		[SerializeField]
		private Tilemap _floorLayer;
		[SerializeField]
		private Tilemap _interactableLayer;

		public InteractableTile GetTileAt(Vector2Int position) {
			return _interactableLayer.GetTile<InteractableTile>(position.SwizzleXZ());
		}

		public T GetInteractableAt<T>(Vector2Int position) where T : class {
			return null;
		}

		public bool IsWalkable(Vector2Int position) {
			var position3d = position.SwizzleXY();
			if (!_floorLayer.GetTile(position3d))
				return false;

			var tile = _interactableLayer.GetTile(position3d);
			return !tile || tile is not InteractableTile it || it.IsWalkable();
		}

		internal Vector2Int WorldToGrid(Vector3 position3d) {
			return _interactableLayer.WorldToCell(position3d).SwizzleXY();
		}

		internal Vector3 GridToWorld(Vector2Int position2d) {
			var position3d = _interactableLayer.CellToWorld(position2d.SwizzleXY());
			position3d.x += _interactableLayer.tileAnchor.x;
			position3d.z += _interactableLayer.tileAnchor.y;
			return position3d;
		}
	}
}
