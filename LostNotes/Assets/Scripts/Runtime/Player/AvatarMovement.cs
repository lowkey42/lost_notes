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

		public void OnPlaySong(SongAsset song) {
			song.PlaySong(gameObject);
		}

		public void OnFailSong(SongAsset song) {
			song.PlaySong(gameObject);
		}
	}
}
