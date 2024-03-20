using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class NoteAsset : ScriptableObject {
		[SerializeField]
		private string _localizedName = "C";
		public string LocalizedName => _localizedName;

		[SerializeField]
		private InputActionReference _inputAction;
		[SerializeField]
		private EventReference _audioEvent = new();

		public Guid Identifier => _inputAction.action.id;

		internal bool Is(InputAction action) {
			return Identifier == action.id;
		}

		internal bool Is(NoteAsset note) {
			return Identifier == note.Identifier;
		}

		internal EventInstance Play() {
			var instance = RuntimeManager.CreateInstance(_audioEvent);
			_ = instance.start();
			return instance;
		}
	}
}
