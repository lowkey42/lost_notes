using System.Collections.Generic;
using LostNotes.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class NoteHud : MonoBehaviour, ISongMessages {
		[SerializeField]
		private Image _image;

		private readonly List<SongAsset> _songs = new();

		public void OnSetSong(SongAsset song) {
			_songs.Add(song);
			song.OnChangeLearned += HandleLearned;
			HandleLearned(song.NotesLearned);
		}

		private void OnDestroy() {
			foreach (var song in _songs) {
				song.OnChangeLearned -= HandleLearned;
			}

			_songs.Clear();
		}

		private void HandleLearned(int value) {
			_image.color = value >= transform.GetSiblingIndex()
				? Color.white
				: default;
		}
	}
}
