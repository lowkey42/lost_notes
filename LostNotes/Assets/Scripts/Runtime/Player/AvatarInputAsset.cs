using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class AvatarInputAsset : ScriptableObject {
		public event Action<InputActionReference> OnActionInput;
		public event Action<Vector2Int> OnMoveInput;
		public event Action OnSkipTurn;
		public event Action OnTogglePlay;
		public event Action OnTogglePause;
		public event Action OnReset;

		public void RaiseInput(InputActionReference input) {
			OnActionInput?.Invoke(input);
		}

		public void RaiseMoveX(int input) {
			OnMoveInput?.Invoke(new Vector2Int(input, 0));
		}

		public void RaiseMoveY(int input) {
			OnMoveInput?.Invoke(new Vector2Int(0, input));
		}

		public void RaiseTogglePlay() {
			OnTogglePlay?.Invoke();
		}

		public void RaiseTogglePause() {
			OnTogglePause?.Invoke();
		}

		public void RaiseSkipTurn() {
			OnSkipTurn?.Invoke();
		}

		public void RaiseReset() {
			OnReset?.Invoke();
		}

		private GameObject _menu;
		public void ToggleMenu(GameObject prefab) {
			if (_menu) {
				Destroy(_menu);
			} else {
				_menu = Instantiate(prefab);
			}
		}
	}
}
