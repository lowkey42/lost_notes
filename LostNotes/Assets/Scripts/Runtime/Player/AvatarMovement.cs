using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class AvatarMovement : MonoBehaviour, IAvatarMessages {
		private Vector2Int Position {
			get => Vector2Int.RoundToInt(transform.position.SwizzleXZ());
			set => transform.position = value.SwizzleXZ();
		}

		public void MoveBy(Vector2Int delta) {
			Position += delta;
		}

		public void PlaySong(SongAsset song) {
			song.PlaySong(gameObject);
		}

		public void FailSong(SongAsset song) {
			song.PlaySong(gameObject);
		}
	}
}
