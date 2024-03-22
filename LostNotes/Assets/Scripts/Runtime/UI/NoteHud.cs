using LostNotes.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class NoteHud : MonoBehaviour {
		[SerializeField]
		private Image _unknown;
		[SerializeField]
		private Image _learned;
		[SerializeField]
		private Image _playing;

		private SongAsset _song;
		private int _noteIndex;

		internal void SetUpNote(NoteAsset note, SongAsset song, int i) {
			_noteIndex = i;

			(_playing.transform.parent as RectTransform).pivot = new Vector2(0.5f, Mathf.InverseLerp(-6, 6, note.StepsToB));
			var rotation = new Vector3(0, 0, note.InputRotation);
			_learned.transform.localEulerAngles = rotation;
			_playing.transform.localEulerAngles = rotation;

			ResetSong();

			_song = song;
			song.OnChangeLearned += HandleLearned;
			song.OnChangePlayed += HandlePlayed;
			HandleLearned(0);
		}

		private void ResetSong() {
			if (_song) {
				_song.OnChangeLearned -= HandleLearned;
				_song.OnChangePlayed -= HandlePlayed;
				_song = null;
			}
		}

		private void OnDestroy() {
			ResetSong();
		}

		private bool _isLearned;
		private bool _isPlayed;

		private void HandleLearned(int value) {
			_isLearned = value > _noteIndex;
			UpdateImages();
		}

		private void HandlePlayed(int value) {
			_isPlayed = value > _noteIndex;
			UpdateImages();
		}

		private void UpdateImages() {
			(_unknown.enabled, _learned.enabled, _playing.enabled) = (_isLearned, _isPlayed) switch {
				(true, true) => (false, false, true),
				(true, _) => (false, true, false),
				(_, _) => (true, false, false),
			};
		}
	}
}
