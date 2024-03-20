using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal static class TilemapExtensions {
		public static bool TryGetTileMeta(this Tilemap tilemap, Vector2Int position, out ITileMeta tile) {
			return TryGetTileMeta(tilemap, position.SwizzleXY(), out tile);
		}
		public static bool TryGetTileMeta(this Tilemap tilemap, Vector3Int position, out ITileMeta tile) {
			tile = tilemap.GetTile(position) as ITileMeta;
			return tile is not null;
		}
	}
}
