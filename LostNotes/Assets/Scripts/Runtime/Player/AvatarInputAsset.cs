using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class AvatarInputAsset : ScriptableObject {
		public event Action<InputActionReference> OnInput;
		public event Action OnTogglePlay;

		public void RaiseInput(InputActionReference input) {
			OnInput?.Invoke(input);
		}

		public void RaiseTogglePlay() {
			OnTogglePlay?.Invoke();
		}
	}
}
