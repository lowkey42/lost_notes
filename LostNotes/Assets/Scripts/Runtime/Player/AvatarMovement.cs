using LostNotes.Gameplay;
using LostNotes.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostNotes.Player {
	[RequireComponent(typeof(LevelGridTransform))]
	internal sealed class AvatarMovement : MonoBehaviour, IAvatarMessages {
		[FormerlySerializedAs("_movement")]
		[SerializeField]
		private LevelGridTransform _levelGridTransform;

		private void Start() {
			OnValidate();
		}

		private void OnValidate() {
			if (!_levelGridTransform)
				_levelGridTransform = GetComponentInChildren<LevelGridTransform>();
		}

		public void OnMove(Vector2Int delta) {
			_ = _levelGridTransform.MoveBy(delta);
		}

		[SerializeField]
		private TilemapMask _songRange = new(new Vector2Int(9, 9));

		public void OnPlaySong(SongAsset song) {
			song.PlaySong(_levelGridTransform, _songRange);
		}

		public void OnReset() {
		}
	}
}
