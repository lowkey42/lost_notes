using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
			if (EventSystem.current) {
				if (_menu.transform.TryGetComponentInChildren<Selectable>(out var selectable)) {
					EventSystem.current.SetSelectedGameObject(selectable.gameObject);
				}
			}
		}

		private void CloseMenu() {
			if (!_menu) {
				return;
			}

			Destroy(_menu);
			_menu = null;
		}
	}
}
