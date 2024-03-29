using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal sealed class LevelComponent : MonoBehaviour {
		public event Action<Vector2Int, TileBase> OnSetHighlight;
		public event Action<Vector2Int> OnClearHighlight;

		[SerializeField, Expandable]
		private Tilemap _floorLayer;

		public void SetHighlighting(IEnumerable<Vector2Int> positions, TileBase tile) {
			foreach (var position in positions) {
				OnSetHighlight?.Invoke(position, tile);
			}
		}

		public void ClearHighlighting(IEnumerable<Vector2Int> positions) {
			foreach (var position in positions) {
				OnClearHighlight?.Invoke(position);
			}
		}

		[SerializeField, Expandable]
		private Tilemap _interactableLayer;

		[SerializeField, Expandable]
		private Tilemap _wallLayer;

		[SerializeField, Expandable]
		private SongLoadout _songLoadout;

		private void Start() {
			_songLoadout.Load();
		}

		private void OnDestroy() {
			_songLoadout.Reset();
		}

		// TODO: TilesInArea and ObjectsInArea currently ignore if tiles are shaded by walls

		public IEnumerable<Vector2Int> TilesInArea(Vector2Int position, int rotation, TilemapMask area) {
			var rotation3d = Quaternion.AngleAxis(rotation, Vector3.up);

			for (var y = 0; y < area.Size.y; y++) {
				for (var x = 0; x < area.Size.x; x++) {
					var localGridPosition = new Vector2Int(x, y);
					var worldGridPosition = Vector2Int.RoundToInt((rotation3d * (localGridPosition - (area.Size / 2)).SwizzleXZ()).SwizzleXZ()) + position;
					if (area.IsSet(localGridPosition) && IsInteractionUnblocked(position, worldGridPosition))
						yield return worldGridPosition;
				}
			}
		}

		public IEnumerable<GameObject> ObjectsInArea(LevelGridTransform source, TilemapMask area) {
			foreach (Transform t in _interactableLayer.transform) {
				if (!t.gameObject.activeInHierarchy || t == source.transform)
					continue;

				var testPosition = WorldToGrid(t.position);
				var localPosition = source.WorldToLocalPosition(testPosition) + (area.Size / 2);

				if (area.IsSet(localPosition) && IsInteractionUnblocked(source.Position2d, testPosition))
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

		public bool IsWalkable(Vector2Int position, Transform ignore = null) {
			return _floorLayer.GetTile(position.SwizzleXY()) && !_wallLayer.GetTile(position.SwizzleXY()) &&
			       _interactableLayer.GetMetaTiles(position, ignore).All(t => t.IsWalkable);
		}

		public bool IsInteractionBlocking(Vector2Int position) {
			return !_floorLayer.GetTile(position.SwizzleXY()) || _wallLayer.GetTile(position.SwizzleXY()) ||
				   _interactableLayer.GetMetaTiles(position).Any(t => t.IsInteractionBlocking);
		}

		public bool IsInteractionUnblocked(Vector2Int a, Vector2Int b) {
			return IsInteractionUnblocked(a, b, out var _);
		}

		public bool IsInteractionUnblocked(Vector2Int a, Vector2Int b, out Vector2Int lastUnblocked) {
			// Raycast check of all tiles between a and b, using Bresenham's line algorithm
			var deltaError = new Vector2Int(Mathf.Abs(b.x - a.x), -Mathf.Abs(b.y - a.y));
			var step = new Vector2Int(a.x < b.x ? 1 : -1, a.y < b.y ? 1 : -1);
			var error = deltaError.x + deltaError.y;
			var firstIteration = true;
			lastUnblocked = a;
			for (;;) {
				if (a.x == b.x && a.y == b.y) {
					lastUnblocked = b;
					return true;
				}

				if (!firstIteration && IsInteractionBlocking(a)) 
					return false;
				lastUnblocked = a;

				var error2 = 2 * error;
				if (error2 >= deltaError.y) {
					if (a.x == b.x)
						return true;
					error += deltaError.y;
					a.x += step.x;
				}

				if (error2 <= deltaError.x) {
					if (a.y == b.y) 
						return true;
					error += deltaError.x;
					a.y += step.y;
				}

				firstIteration = false;
			}
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
