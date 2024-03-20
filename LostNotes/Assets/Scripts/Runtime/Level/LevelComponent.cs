using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal sealed class LevelComponent : MonoBehaviour {
		[SerializeField]
		private Tilemap _floorLayer;
		[SerializeField]
		private Tilemap _interactableLayer;
		[SerializeField]
		private Tilemap _wallLayer;

		public IEnumerable<ITileMeta> GetInteractableTiles(Vector2Int position) {
			return _interactableLayer.GetMetaTiles(position);
		}

		public bool IsWalkable(Vector2Int position) {
			return _floorLayer.GetTile(position.SwizzleXY())
				&& !_wallLayer.GetTile(position.SwizzleXY())
				&& _interactableLayer.GetMetaTiles(position).All(t => t.IsWalkable);
		}

		public Vector2Int WorldToGrid(Vector3 position3d) {
			return _interactableLayer.WorldToCell(position3d).SwizzleXY();
		}

		public Vector3 GridToWorld(Vector2Int position2d) {
			var position3d = _interactableLayer.CellToWorld(position2d.SwizzleXY());
			position3d.x += _interactableLayer.tileAnchor.x;
			position3d.z += _interactableLayer.tileAnchor.y;
			return position3d;
		}
	}
}
