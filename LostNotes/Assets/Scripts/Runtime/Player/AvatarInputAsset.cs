using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class AvatarInputAsset : ScriptableObject {
		public event Action<InputActionReference> OnActionInput;
		public event Action<Vector2Int> OnMoveInput;
		public event Action OnTogglePlay;

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
	}
}
