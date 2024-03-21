using System.Collections.Generic;
using LostNotes.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class SongHud : MonoBehaviour, ISongMessages {
		[SerializeField]
		private Image _image;

		private readonly List<SongAsset> _songs = new();

		public void OnSetSong(SongAsset song) {
			_songs.Add(song);
			song.onChangeAvailable += HandleAvailable;
			song.SetUpHud(_image);
		}

		private void OnDestroy() {
			foreach (var song in _songs) {
				song.onChangeAvailable -= HandleAvailable;
			}

			_songs.Clear();
		}

		private void HandleAvailable(bool isAvailable) {
			_image.enabled = isAvailable;
		}
	}
}
