using System;
using System.Collections;
using FMODUnity;
using LostNotes.Gameplay;
using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class SongAsset : ScriptableObject {
		[SerializeField]
		private bool _isAvailable = false;

		public bool IsAvailable {
			get => _isAvailable;
			set => _isAvailable = value;
		}

		[SerializeField]
		private EEffects _effects = EEffects.Noise;
		[SerializeField]
		private float _effectDelay = 0.1f;

		private IEnumerator PlayEffects_Co(LevelGridTransform context, TilemapMask range) {
			yield return Wait.forSeconds[_effectDelay];

			foreach (var message in _effects.GetMessages()) {
				context.SendMessageToObjectsInArea(range, message, context);
			}
		}

		[SerializeField]
		private NoteAsset[] _notes = Array.Empty<NoteAsset>();
		private int _currentNodeIndex = 0;

		public int NoteCount => _notes.Length;
		public bool IsFailure => NoteCount > 0;

		public void Learn() {
			_isAvailable = true;
		}

		public void ResetInput() {
			_currentNodeIndex = 0;
		}

		public ESongStatus PlayNote(NoteAsset note) {
			if (!_isAvailable) {
				return ESongStatus.NotLearned;
			}

			if (_currentNodeIndex == _notes.Length) {
				return ESongStatus.Failed;
			}

			if (_notes[_currentNodeIndex].Is(note)) {
				_currentNodeIndex++;
				return _currentNodeIndex == _notes.Length
					? ESongStatus.Done
					: ESongStatus.Playing;
			}

			_currentNodeIndex = _notes.Length;
			return ESongStatus.Failed;
		}

		[SerializeField]
		private EventReference _songEvent = new();
		[SerializeField]
		private GameObject _songPrefab;
		[SerializeField]
		private Color _songColor = Color.green;
		public Color Color => _songColor;

		public void PlaySong(LevelGridTransform context, TilemapMask range) {
			if (!_songEvent.IsNull) {
				RuntimeManager.PlayOneShot(_songEvent);
			}

			if (_songPrefab) {
				var instance = Instantiate(_songPrefab, context.transform);
				instance.BroadcastMessage(nameof(ISongMessages.OnStartSong), this, SendMessageOptions.DontRequireReceiver);
			}

			_ = context.StartCoroutine(PlayEffects_Co(context, range));
		}
	}
}
