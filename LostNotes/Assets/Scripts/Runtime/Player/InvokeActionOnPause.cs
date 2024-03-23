using UnityEngine;
using UnityEngine.Events;

namespace LostNotes.Player {
	internal sealed class InvokeActionOnPause : MonoBehaviour, IAvatarMessages {
		[SerializeField]
		private UnityEvent _onPause = new();

		public void OnMove(Vector2Int delta) {
		}

		public void OnPlaySong(SongAsset song) {
		}

		public void OnReset() {
		}

		public void OnPause() {
			_onPause.Invoke();
		}

		public void OnSkip() {
		}
	}
}
