using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class SongPlayer : MonoBehaviour, INoteMessages {

		[SerializeField]
		private string _songLabel;
		[SerializeField, ReadOnly]
		private List<SongAsset> _songs = new();
		[SerializeField]
		private SongAsset _failureSong;

		private void AddSong(SongAsset song) {
			_songs.Add(song);
		}

		private IEnumerator LoadSongs() {
			var locationHandle = Addressables.LoadResourceLocationsAsync(_songLabel);
			yield return locationHandle;

			yield return Addressables.LoadAssetsAsync<SongAsset>(locationHandle.Result, AddSong);
		}

		private IEnumerator Start() {
			yield return LoadSongs();
		}

		public void StartPlaying() {
			foreach (var song in _songs) {
				song.ResetInput();
			}
		}

		public void StopPlaying() {
		}

		public void StartNote(InputAction action) {
			var isPlayingAny = false;
			foreach (var song in _songs) {
				var status = song.PlayNote(action);
				if (status is ESongStatus.Playing or ESongStatus.Done) {
					isPlayingAny = true;
				}

				if (status is ESongStatus.Done) {
					gameObject.BroadcastMessage(nameof(IAvatarMessages.PlaySong), song, SendMessageOptions.DontRequireReceiver);
				}
			}

			if (!isPlayingAny) {
				gameObject.BroadcastMessage(nameof(IAvatarMessages.FailSong), _failureSong, SendMessageOptions.DontRequireReceiver);
			}
		}

		public void StopNote(InputAction action) {
		}
	}
}
