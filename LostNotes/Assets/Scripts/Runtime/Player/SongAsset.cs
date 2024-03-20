using System;
using FMODUnity;
using UnityEngine;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class SongAsset : ScriptableObject {
		[SerializeField]
		private bool _isAvailable = false;
		[SerializeField]
		private NoteAsset[] _notes = Array.Empty<NoteAsset>();
		private int _currentNodeIndex = 0;

		public int NoteCount => _notes.Length;

		public void Learn() {
			_isAvailable = true;
		}

		public void ResetInput() {
			_currentNodeIndex = 0;
		}

		public ESongStatus PlayNote(NoteAsset note) {
			if (!_isAvailable) {
				return ESongStatus.NotLearned;
			}

			if (_currentNodeIndex == _notes.Length) {
				return ESongStatus.Failed;
			}

			if (_notes[_currentNodeIndex].Is(note)) {
				_currentNodeIndex++;
				return _currentNodeIndex == _notes.Length
					? ESongStatus.Done
					: ESongStatus.Playing;
			}

			_currentNodeIndex = _notes.Length;
			return ESongStatus.Failed;
		}

		[SerializeField]
		private EventReference _songEvent = new();
		[SerializeField]
		private GameObject _songPrefab;

		public void PlaySong(GameObject context) {
			if (!_songEvent.IsNull) {
				RuntimeManager.PlayOneShot(_songEvent);
			}

			if (_songPrefab) {
				_ = Instantiate(_songPrefab, context.transform);
			}
		}
	}
}
