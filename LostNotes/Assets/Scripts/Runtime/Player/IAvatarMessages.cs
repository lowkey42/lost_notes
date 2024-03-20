using UnityEngine;

namespace LostNotes.Player {
	internal interface IAvatarMessages {
		void OnMove(Vector2Int delta);
		void OnPlaySong(SongAsset song);
		void OnFailSong(SongAsset song);
		void OnReset();
	}
}
