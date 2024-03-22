using UnityEngine;

namespace LostNotes.Player {
	internal sealed class InstantiatePrefabOnNote : MonoBehaviour, INoteMessages {
		[SerializeField]
		private GameObject _prefab;

		public void OnStartPlaying() {
		}

		public void OnStopPlaying() {
		}

		public void OnStartNote(NoteAsset note) {
			_ = Instantiate(_prefab, transform);
		}

		public void OnStopNote(NoteAsset note) {
		}
	}
}
