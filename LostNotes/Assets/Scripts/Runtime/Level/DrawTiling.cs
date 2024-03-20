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
			foreach (var position in _level.GetFloorPositions()) {
				_tilemap.SetTile(position, _tileToDraw);
			}
		}

		private void OnDisable() {
			_tilemap.ClearAllTiles();
		}
	}
}
