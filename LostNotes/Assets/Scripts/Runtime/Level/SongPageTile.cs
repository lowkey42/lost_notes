using LostNotes.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	[CreateAssetMenu]
	internal sealed class SongPageTile : TileBase, ITileMeta {
		[SerializeField]
		private Sprite _sprite;
		[SerializeField]
		private GameObject _prefab;
		[SerializeField, Expandable]
		private SongAsset _song;

		public bool IsWalkable => true;
		public bool IsInteractionBlocking => false;

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			tileData.sprite = _sprite;
			tileData.gameObject = _prefab;
			tileData.flags = TileFlags.LockAll;
			tileData.color = _song.Color;
		}

		public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
			if (go && Application.isPlaying) {
				go.BroadcastMessage(nameof(ISongMessages.OnSetSong), _song, SendMessageOptions.DontRequireReceiver);
			}

			return base.StartUp(position, tilemap, go);
		}
	}
}
