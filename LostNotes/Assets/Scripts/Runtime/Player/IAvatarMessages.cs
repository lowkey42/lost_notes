using UnityEngine;

namespace LostNotes.Player {
	internal interface IAvatarMessages {
		void MoveBy(Vector2Int delta);
		void PlaySong(SongAsset song);
		void FailSong(SongAsset song);
	}
}
