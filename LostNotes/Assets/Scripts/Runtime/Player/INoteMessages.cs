using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal interface INoteMessages {
		void OnStartPlaying();
		void OnStopPlaying();
		void OnStartNote(InputAction action);
		void OnStopNote(InputAction action);
	}
}
