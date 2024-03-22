using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using LostNotes.Gameplay;
using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.Player {
	[CreateAssetMenu]
	internal sealed class SongAsset : ScriptableObject {
		public event Action<bool> OnChangeAvailable;
		public event Action<int> OnChangeLearned;

		[SerializeField]
		private bool _isAvailable = false;

		public bool IsAvailable {
			get => _isAvailable;
			set {
				_isAvailable = value;
				OnChangeAvailable?.Invoke(value);
			}
		}

		[SerializeField]
		private int _notesLearned = 0;
		public int NotesLearned {
			get => _notesLearned;
			set {
				_notesLearned = value;
				OnChangeLearned?.Invoke(value);
			}
		}

		[SerializeField]
		private EEffects _effects = EEffects.Noise;
		[SerializeField]
		private float _effectDelay = 0.1f;
		[SerializeField]
		private EObjectSorting _receiverSorting = EObjectSorting.None;

		private IEnumerator PlayEffects_Co(LevelGridTransform context, TilemapMask range) {
			yield return Wait.forSeconds[_effectDelay];

			var receivers = context.ObjectsInArea(range);

			switch (_receiverSorting) {
				case EObjectSorting.None:
					break;
				case EObjectSorting.ClosestToSource:
					receivers = receivers.OrderBy(obj => Vector3.Distance(obj.transform.position, context.transform.position));
					break;
				case EObjectSorting.FurthestToSource:
					receivers = receivers.OrderByDescending(obj => Vector3.Distance(obj.transform.position, context.transform.position));
					break;
				default:
					throw new NotImplementedException();
			}

			foreach (var obj in receivers) {
				foreach (var message in _effects.GetMessages()) {
					obj.BroadcastMessage(message, context, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		[SerializeField]
		private NoteAsset[] _notes = Array.Empty<NoteAsset>();
		private int _currentNodeIndex = 0;

		public IEnumerable<NoteAsset> Notes => _notes;
		public int NoteCount => _notes.Length;
		public bool IsFailure => NoteCount == 0;

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
				NotesLearned = Mathf.Max(NotesLearned, _currentNodeIndex);
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
		[SerializeField]
		private Sprite _hudSprite;
		public Sprite HudSprite => _hudSprite;

		public IEnumerator PlaySong(LevelGridTransform context, TilemapMask range) {
			if (!_songEvent.IsNull) {
				RuntimeManager.PlayOneShot(_songEvent);
			}

			if (_songPrefab) {
				var instance = Instantiate(_songPrefab, context.transform.position, Quaternion.identity);
				instance.BroadcastMessage(nameof(ISongMessages.OnSetSong), this, SendMessageOptions.DontRequireReceiver);
			}

			yield return PlayEffects_Co(context, range);
		}
	}
}
