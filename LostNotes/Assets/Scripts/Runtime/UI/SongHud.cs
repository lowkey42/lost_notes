using System.Collections.Generic;
using LostNotes.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class SongHud : MonoBehaviour, ISongMessages {
		[SerializeField]
		private Image _image;
		[SerializeField]
		private GameObject _notePrefab;

		private readonly List<SongAsset> _songs = new();

		public void OnSetSong(SongAsset song) {
			_songs.Add(song);
			song.OnChangeAvailable += HandleAvailable;
			_image.sprite = song.HudSprite;

			transform.Clear();
			foreach (var note in song.Notes) {
				var instance = Instantiate(_notePrefab, transform);
				instance.BroadcastMessage(nameof(INoteMessages.OnStartNote), note, SendMessageOptions.DontRequireReceiver);
				instance.BroadcastMessage(nameof(ISongMessages.OnSetSong), song, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void OnDestroy() {
			foreach (var song in _songs) {
				song.OnChangeAvailable -= HandleAvailable;
			}

			_songs.Clear();
		}

		private void HandleAvailable(bool isAvailable) {
			_image.enabled = isAvailable;
		}
	}
}
