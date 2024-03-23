using System;
using FMODUnity;
using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class SongPlayer : MonoBehaviour, INoteMessages {
		[SerializeField, Expandable]
		private SongLoadout _validSongs;
		[SerializeField, Expandable]
		private SongAsset[] _failureSongs = Array.Empty<SongAsset>();

		private void ResetSongs() {
			_songStatus = ESongStatus.NotLearned;
			foreach (var song in _validSongs.Songs) {
				song.ResetInput();
			}
		}

		public void OnStartPlaying() {
			ResetSongs();
		}

		public void OnStopPlaying() {
			foreach (var song in _validSongs.Songs) {
				song.ResetPlaying();
			}
		}

		private ESongStatus _songStatus = ESongStatus.NotLearned;
		private SongAsset _song;

		[SerializeField]
		private GameObject _notePrefab;

		public void OnStartNote(NoteAsset note) {
			var isPlayingAny = false;
			foreach (var song in _validSongs.Songs) {
				var status = song.PlayNote(note);
				if (status is ESongStatus.Playing or ESongStatus.Done) {
					isPlayingAny = true;
				}

				if (status is ESongStatus.Done) {
					_songStatus = ESongStatus.Done;
					_song = song;
				}
			}

			if (isPlayingAny && _songStatus != ESongStatus.Done) {
				_ = Instantiate(_notePrefab, transform);
			}

			if (!isPlayingAny) {
				_songStatus = ESongStatus.Failed;
			}
		}

		[SerializeField, ParamRef]
		private string _hasPlayedParameter = "";

		private void Update() {
			switch (_songStatus) {
				case ESongStatus.Done:
					_songStatus = ESongStatus.NotLearned;

					if (!string.IsNullOrEmpty(_hasPlayedParameter)) {
						_ = RuntimeManager.StudioSystem.setParameterByNameWithLabel(_hasPlayedParameter, "True");
					}

					gameObject.BroadcastMessage(nameof(IAvatarMessages.OnPlaySong), _song, SendMessageOptions.DontRequireReceiver);
					break;
				case ESongStatus.Failed:
					_songStatus = ESongStatus.NotLearned;
					gameObject.BroadcastMessage(nameof(IAvatarMessages.OnPlaySong), _failureSongs.RandomElement(), SendMessageOptions.DontRequireReceiver);
					break;
			}
		}

		public void OnStopNote(NoteAsset note) {
		}
	}
}
