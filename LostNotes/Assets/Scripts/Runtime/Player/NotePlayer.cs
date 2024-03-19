using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class NotePlayer : MonoBehaviour, INoteMessages {
		[SerializeField]
		private InputActionReference[] _noteActions = Array.Empty<InputActionReference>();
		[SerializeField]
		private EventReference[] _noteEvents = Array.Empty<EventReference>();

		private readonly Dictionary<Guid, EventInstance> _instances = new();

		public void StartPlaying() {
		}

		public void StopPlaying() {
			foreach (var action in _noteActions) {
				StopNote(action);
			}
		}

		public void StartNote(InputAction action) {
			for (var i = 0; i < _noteActions.Length; i++) {
				var actionReference = _noteActions[i];
				var eve = _noteEvents[i];

				if (actionReference.action.id == action.id && !eve.IsNull) {
					StopNote(action);
					_instances[action.id] = RuntimeManager.CreateInstance(eve);
					_ = _instances[action.id].start();
				}
			}
		}

		[SerializeField]
		private bool _stopPlayingNotes = false;

		public void StopNote(InputAction action) {
			if (_instances.Remove(action.id, out var instance)) {
				if (_stopPlayingNotes) {
					_ = instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				}
			}
		}
	}
}
