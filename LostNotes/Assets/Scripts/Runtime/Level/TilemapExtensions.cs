using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal static class TilemapExtensions {
		private sealed class TileWithCollision : ITileMeta {
			public bool IsWalkable => false;
			public bool IsInteractionBlocking => true;
		}

		public static IEnumerable<ITileMeta> GetMetaTiles(this Tilemap tilemap, Vector2Int position, Transform ignore = null) {
			return GetMetaTiles(tilemap, position.SwizzleXY(), ignore);
		}

		public static IEnumerable<ITileMeta> GetMetaTiles(this Tilemap tilemap, Vector3Int position, Transform ignore = null) {
			if (tilemap.TryGetMetaTile(position, out var tile)) {
				yield return tile;
			}

			foreach (Transform t in tilemap.transform) {
				if (t.gameObject.activeInHierarchy && t != ignore && tilemap.WorldToCell(t.position) == position && t.TryGetComponent(out tile)) yield return tile;
			}
		}

		public static bool TryGetMetaTile(this Tilemap tilemap, Vector2Int position, out ITileMeta tile) {
			return TryGetMetaTile(tilemap, position.SwizzleXY(), out tile);
		}
		public static bool TryGetMetaTile(this Tilemap tilemap, Vector3Int position, out ITileMeta tile) {
			tile = tilemap.GetTile(position) as ITileMeta;

			if (tile is null) {
				if (tilemap.GetColliderType(position) == Tile.ColliderType.Grid) {
					tile = new TileWithCollision();
				}
			}

			return tile is not null;
		}
	}
}
