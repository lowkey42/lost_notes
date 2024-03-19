using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class NoteCanvas : MonoBehaviour, INoteMessages, IAvatarMessages {
		[SerializeField]
		private Canvas _attachedCanvas;
		[SerializeField]
		private TextMeshProUGUI _attachedText;

		private List<string> _notes = new();

		private void AddNote(string note) {
			_notes.Add(note);
			UpdateText();
		}

		private void UpdateText() {
			_attachedText.text = string.Join(' ', _notes);
		}

		public void StartPlaying() {
			if (_stopRoutine is not null) {
				StopCoroutine(_stopRoutine);
			}

			_attachedCanvas.enabled = true;
			_notes.Clear();
			UpdateText();
			_attachedText.color = _defaultColor;
		}

		private Coroutine _stopRoutine;

		public void StopPlaying() {
			IEnumerator Stop() {
				yield return Wait.forSeconds[1];
				_attachedCanvas.enabled = false;
			}

			_stopRoutine = StartCoroutine(Stop());
		}

		public void StartNote(InputAction action) {
			if (_clearOnNextNote) {
				_clearOnNextNote = false;
				StartPlaying();
			}

			AddNote(action.name);
		}

		public void StopNote(InputAction action) {
		}

		public void MoveBy(Vector2Int delta) {
		}

		[SerializeField]
		private Color _defaultColor = Color.white;
		[SerializeField]
		private Color _playColor = Color.green;
		[SerializeField]
		private Color _failColor = Color.red;

		private bool _clearOnNextNote = false;

		public void PlaySong(SongAsset song) {
			_notes = new(_notes.TakeLast(song.NoteCount));
			UpdateText();
			_attachedText.color = _playColor;
			_clearOnNextNote = true;
		}

		public void FailSong(SongAsset song) {
			_attachedText.color = _failColor;
			_clearOnNextNote = true;
		}
	}
}
