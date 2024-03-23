using UnityEngine;

namespace LostNotes.Player {
	internal sealed class InvokeMenuOnPause : MonoBehaviour, IAvatarMessages {
		[SerializeField]
		private AvatarMenuAsset _menus;

		public void OnMove(Vector2Int delta) {
		}

		public void OnPlaySong(SongAsset song) {
		}

		public void OnReset() {
		}

		public void OnPause(bool isPaused) {
			_menus.TogglePauseMenu(isPaused);
		}

		public void OnSkip() {
		}
	}
}
