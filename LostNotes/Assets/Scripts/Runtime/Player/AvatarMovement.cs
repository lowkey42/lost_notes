using UnityEngine;

namespace LostNotes.Player {
	[RequireComponent(typeof(Movement))]
	internal sealed class AvatarMovement : MonoBehaviour, IAvatarMessages {
		[SerializeField]
		private Movement _movement;

		private void Start() {
			OnValidate();
		}

		private void OnValidate() {
			if (!_movement)
				_movement = GetComponentInChildren<Movement>();
		}

		public void OnMove(Vector2Int delta) {
			_ = _movement.MoveBy(delta);
		}

		public void OnPlaySong(SongAsset song) {
			song.PlaySong(gameObject);
		}

		public void OnFailSong(SongAsset song) {
			song.PlaySong(gameObject);
		}
	}
}
