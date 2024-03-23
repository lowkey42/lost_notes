using UnityEngine;

namespace LostNotes {
	[CreateAssetMenu]
	internal sealed class AvatarMenuAsset : ScriptableObject {
		[SerializeField]
		private GameObject _pauseMenuPrefab;
		public void TogglePauseMenu(bool isPaused) {
			if (isPaused) {
				OpenMenu(_pauseMenuPrefab);
			} else {
				CloseMenu();
			}
		}

		private GameObject _menu;

		private void OpenMenu(GameObject _prefab) {
			if (_menu) {
				CloseMenu();
			}

			_menu = Instantiate(_prefab);
		}

		private void CloseMenu() {
			if (!_menu) {
				return;
			}

			Destroy(_menu);
			_menu = null;
		}

		public void QuitGame() {
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#endif
		}
	}
}
