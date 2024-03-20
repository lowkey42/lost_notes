using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal static class TilemapExtensions {
		private sealed class TileWithCollision : ITileMeta {
			public bool IsWalkable => false;
		}

		public static bool TryGetTileMeta(this Tilemap tilemap, Vector2Int position, out ITileMeta tile) {
			return TryGetTileMeta(tilemap, position.SwizzleXY(), out tile);
		}
		public static bool TryGetTileMeta(this Tilemap tilemap, Vector3Int position, out ITileMeta tile) {
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
