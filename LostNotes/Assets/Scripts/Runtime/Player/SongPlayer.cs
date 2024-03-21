using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class SongPlayer : MonoBehaviour, INoteMessages {

		[SerializeField]
		private string _songLabel;
		[SerializeField, ReadOnly]
		private List<SongAsset> _songs = new();
		[SerializeField]
		private SongAsset _failureSong;

		private IEnumerator Start() {
			yield return _songLabel.LoadAssetsAsync<SongAsset>(_songs.Add);
		}

		public void OnStartPlaying() {
			foreach (var song in _songs) {
				song.ResetInput();
			}
		}

		public void OnStopPlaying() {
		}

		private ESongStatus songStatus = ESongStatus.NotLearned;
		private SongAsset song;

		public void OnStartNote(NoteAsset note) {
			var isPlayingAny = false;
			foreach (var song in _songs) {
				var status = song.PlayNote(note);
				if (status is ESongStatus.Playing or ESongStatus.Done) {
					isPlayingAny = true;
				}

				if (status is ESongStatus.Done) {
					songStatus = ESongStatus.Done;
					this.song = song;
				}
			}

			if (!isPlayingAny) {
				songStatus = ESongStatus.Failed;
			}
		}

		private void Update() {
			switch (songStatus) {
				case ESongStatus.Done:
					songStatus = ESongStatus.NotLearned;
					gameObject.BroadcastMessage(nameof(IAvatarMessages.OnPlaySong), song, SendMessageOptions.DontRequireReceiver);
					break;
				case ESongStatus.Failed:
					songStatus = ESongStatus.NotLearned;
					gameObject.BroadcastMessage(nameof(IAvatarMessages.OnPlaySong), _failureSong, SendMessageOptions.DontRequireReceiver);
					break;
			}
		}

		public void OnStopNote(NoteAsset note) {
		}
	}
}
