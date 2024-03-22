using LostNotes.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class NoteHud : MonoBehaviour {
		[SerializeField]
		private Image _image;

		private SongAsset _song;
		private int _noteIndex;

		internal void SetUpNote(NoteAsset note, SongAsset song, int i) {
			_noteIndex = i;

			(_image.transform.parent as RectTransform).pivot = new Vector2(0.5f, Mathf.InverseLerp(-6, 6, note.StepsToB));
			_image.transform.localEulerAngles = new(0, 0, note.InputRotation);

			ResetSong();

			_song = song;
			song.OnChangeLearned += HandleLearned;
			HandleLearned(song.NotesLearned);
		}

		private void ResetSong() {
			if (_song) {
				_song.OnChangeLearned -= HandleLearned;
				_song = null;
			}
		}

		private void OnDestroy() {
			ResetSong();
		}

		private void HandleLearned(int value) {
			_image.color = value > _noteIndex
				? Color.white
				: default;
		}
	}
}
