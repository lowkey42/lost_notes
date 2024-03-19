using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal interface INoteMessages {
		void StartPlaying();
		void StopPlaying();
		void StartNote(InputAction action);
		void StopNote(InputAction action);
	}
}
