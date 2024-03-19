using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes {
	internal sealed class NotePlayer : MonoBehaviour, INoteMessages {
		[SerializeField]
		private InputActionReference[] _noteActions = Array.Empty<InputActionReference>();
		[SerializeField]
		private EventReference[] _noteEvents = Array.Empty<EventReference>();

		private bool _isPlaying = false;
		private readonly Dictionary<InputAction, EventInstance> _instances = new();

		public void StartPlaying() {
			_isPlaying = true;
		}

		public void StopPlaying() {
			_isPlaying = false;
			foreach (var action in _noteActions) {
				StopNote(action);
			}
		}

		public void StartNote(InputAction action) {
			if (!_isPlaying) {
				return;
			}

			for (var i = 0; i < _noteActions.Length; i++) {
				var actionReference = _noteActions[i];
				var eve = _noteEvents[i];

				if (actionReference.action == action && !eve.IsNull) {
					StopNote(action);
					_instances[action] = RuntimeManager.CreateInstance(eve);
					_ = _instances[action].start();
				}
			}
		}

		public void StopNote(InputAction action) {
			if (_instances.Remove(action, out var instance)) {
				_ = instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}
	}
}
