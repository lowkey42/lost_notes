using UnityEngine;

namespace LostNotes.Player {
	internal sealed class SongProvider : MonoBehaviour, ISongMessages {
		public SongAsset Song { get; private set; }

		public void OnSetSong(SongAsset song) {
			Song = song;
		}
	}
}
