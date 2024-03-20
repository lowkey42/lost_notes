using System;
using System.Collections.Generic;
using FMOD.Studio;
using MyBox;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class NotePlayer : MonoBehaviour, INoteMessages {
		private readonly Dictionary<Guid, EventInstance> _instances = new();

		public void OnStartPlaying() {
		}

		public void OnStopPlaying() {
			foreach (var instance in _instances.Values) {
				StopInstance(instance);
			}

			_instances.Clear();
		}

		public void OnStartNote(NoteAsset note) {
			OnStopNote(note);

			_instances[note.Identifier] = note.Play();
		}

		[SerializeField]
		private bool _stopPlayingNotes = false;
		[SerializeField, ConditionalField(nameof(_stopPlayingNotes))]
		private STOP_MODE _stopMode = STOP_MODE.ALLOWFADEOUT;

		public void OnStopNote(NoteAsset note) {
			if (_instances.Remove(note.Identifier, out var instance)) {
				StopInstance(instance);
			}
		}

		private void StopInstance(in EventInstance instance) {
			if (_stopPlayingNotes) {
				_ = instance.stop(_stopMode);
			}
		}
	}
}
