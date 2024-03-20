namespace LostNotes.Player {
	internal interface INoteMessages {
		void OnStartPlaying();
		void OnStopPlaying();
		void OnStartNote(NoteAsset note);
		void OnStopNote(NoteAsset note);
	}
}
