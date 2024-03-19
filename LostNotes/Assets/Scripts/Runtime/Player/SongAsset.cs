using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class SongAsset : ScriptableObject {
		[SerializeField]
		private bool _isAvailable = false;
		[SerializeField]
		private InputActionReference[] _notes = Array.Empty<InputActionReference>();
		private int _currentNodeIndex = 0;

		public void Learn() {
			_isAvailable = true;
		}

		public void ResetInput() {
			_currentNodeIndex = 0;
		}

		public ESongStatus PlayNote(InputAction action) {
			if (!_isAvailable) {
				return ESongStatus.NotLearned;
			}

			if (_notes[_currentNodeIndex].action == action) {
				_currentNodeIndex++;
				if (_currentNodeIndex == _notes.Length) {
					_currentNodeIndex = 0;
					return ESongStatus.Done;
				}

				return ESongStatus.Playing;
			}

			_currentNodeIndex = 0;
			return ESongStatus.Failed;
		}

		[SerializeField]
		private EventReference songEvent = new();
		public void PlaySong() {
			if (songEvent.IsNull) {
				return;
			}

			RuntimeManager.PlayOneShot(songEvent);
		}
	}
}
