using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	[CreateAssetMenu]
	internal sealed class PrefabTile : TileBase, ITileMeta {
		[SerializeField]
		private Sprite sprite;
		[SerializeField]
		private GameObject prefab;
		[SerializeField]
		private bool _isWalkable = true;
		public bool IsWalkable => _isWalkable;

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			base.GetTileData(position, tilemap, ref tileData);

			tileData.sprite = Application.isPlaying
				? null
				: sprite;
			tileData.gameObject = prefab;
			tileData.flags = TileFlags.None;
		}

		public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
			return base.StartUp(position, tilemap, go);
		}
	}
}
