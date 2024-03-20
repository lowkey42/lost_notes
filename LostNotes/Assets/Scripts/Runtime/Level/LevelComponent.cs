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

		// TODO: TilesInArea and ObjectsInArea currently ignore if tiles are shaded by walls

		public IEnumerable<Vector2Int> TilesInArea(Vector2Int position, int rotation, TilemapMask area) {
			var rotation3d = Quaternion.AngleAxis(rotation, Vector3.up);

			for (var y = 0; y < area.Size.y; y++) {
				for (var x = 0; x < area.Size.x; x++) {
					var localGridPosition = new Vector2Int(x, y);
					var worldGridPosition = Vector2Int.RoundToInt((rotation3d * (localGridPosition - (area.Size / 2)).SwizzleXZ()).SwizzleXZ()) + position;
					if (area.IsSet(localGridPosition))
						yield return worldGridPosition;
				}
			}
		}

		public IEnumerable<GameObject> ObjectsInArea(LevelGridTransform source, TilemapMask area) {
			foreach (Transform t in _interactableLayer.transform) {
				if (t == source.transform) continue;

				var testPosition = WorldToGrid(t.position);
				var localPosition = source.WorldToLocalPosition(testPosition) + (area.Size / 2);

				if (area.IsSet(localPosition))
					yield return t.gameObject;
			}
		}

		public void SendMessageToObjectsInArea(LevelGridTransform source, TilemapMask area, string methodName, object parameter = null) {
			foreach (var go in ObjectsInArea(source, area))
				go.BroadcastMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
		}

		public IEnumerable<Vector3Int> GetFloorPositions() {
			foreach (var position in _floorLayer.cellBounds.allPositionsWithin) {
				if (_floorLayer.GetTile(position))
					yield return position;
			}
		}

		public IEnumerable<ITileMeta> GetInteractableTiles(Vector2Int position) {
			return _interactableLayer.GetMetaTiles(position);
		}

		public bool IsWalkable(Vector2Int position) {
			return _floorLayer.GetTile(position.SwizzleXY()) && !_wallLayer.GetTile(position.SwizzleXY()) &&
			       _interactableLayer.GetMetaTiles(position).All(t => t.IsWalkable);
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
