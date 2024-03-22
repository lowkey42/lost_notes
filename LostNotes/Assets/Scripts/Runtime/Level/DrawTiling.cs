using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	internal sealed class DrawTiling : MonoBehaviour {
		[SerializeField]
		private LevelComponent _level;
		[SerializeField]
		private Tilemap _tilemap;
		[SerializeField]
		private TileBase _tileToDraw;

		private void OnEnable() {
			_level.OnSetHighlight += HandleSetHighlight;
			_level.OnClearHighlight += HandleClearHighlight;

			foreach (var position in _level.GetFloorPositions()) {
				_tilemap.SetTile(position, _tileToDraw);
			}
		}

		private void OnDisable() {
			_level.OnSetHighlight -= HandleSetHighlight;
			_level.OnClearHighlight -= HandleClearHighlight;

			_tilemap.ClearAllTiles();
		}

		private void HandleSetHighlight(Vector2Int position, TileBase tile) {
			_tilemap.SetTile(position.SwizzleXY(), tile);
		}

		private void HandleClearHighlight(Vector2Int position) {
			_tilemap.SetTile(position.SwizzleXY(), _tileToDraw);
		}
	}
}
