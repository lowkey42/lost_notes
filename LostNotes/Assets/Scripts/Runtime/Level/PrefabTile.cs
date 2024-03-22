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

		[SerializeField]
		private bool _isInteractionBlocking = false;

		public bool IsInteractionBlocking => _isInteractionBlocking;

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			tileData.sprite = sprite;
			tileData.gameObject = prefab;
			tileData.flags = TileFlags.InstantiateGameObjectRuntimeOnly;
		}

		public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
			if (go) {
				go.transform.eulerAngles = new(0, tilemap.GetTransformMatrix(position).rotation.eulerAngles.z, 0);
			}

			return base.StartUp(position, tilemap, go);
		}
	}
}
