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

		public void OnStartPlaying() {
		}

		public void OnStopPlaying() {
			foreach (var action in _noteActions) {
				OnStopNote(action);
			}
		}

		public void OnStartNote(InputAction action) {
			for (var i = 0; i < _noteActions.Length; i++) {
				var actionReference = _noteActions[i];
				var eve = _noteEvents[i];

				if (actionReference.action.id == action.id && !eve.IsNull) {
					OnStopNote(action);
					_instances[action.id] = RuntimeManager.CreateInstance(eve);
					_ = _instances[action.id].start();
				}
			}
		}

		[SerializeField]
		private bool _stopPlayingNotes = false;

		public void OnStopNote(InputAction action) {
			if (_instances.Remove(action.id, out var instance)) {
				if (_stopPlayingNotes) {
					_ = instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				}
			}
		}
	}
}
