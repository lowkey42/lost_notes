using System.Collections;
using System.Collections.Generic;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class AvatarInput : MonoBehaviour {

		private void OnEnable() {
			SetUpPlayer();
			SetUpNotes();
		}

		private void OnDisable() {
			IsPlaying = false;
			TearDownPlayer();
			TearDownNotes();
		}

		[SerializeField]
		private InputActionAsset playerActions;

		private InputActionMap PlayerMap => playerActions.FindActionMap("Player");

		private void SetUpPlayer() {
			PlayerMap.Enable();

			PlayerMap["Move"].performed += HandleMove;
			PlayerMap["Move"].canceled += HandleMove;
			PlayerMap["Play"].started += HandlePlayStart;
			PlayerMap["Play"].canceled += HandlePlayStop;
		}

		private void TearDownPlayer() {
			PlayerMap["Move"].performed -= HandleMove;
			PlayerMap["Move"].canceled -= HandleMove;
			PlayerMap["Play"].started -= HandlePlayStart;
			PlayerMap["Play"].canceled -= HandlePlayStop;

			PlayerMap.Disable();
		}

		private Vector2Int _lastInput;

		[SerializeField, ReadOnly]
		private bool _canMove = true;
		public bool CanMove {
			get => _canMove && !_isPlaying;
			set => _canMove = value;
		}

		[SerializeField, ReadOnly]
		private bool _canChangePlayState = true;
		public bool CanChangePlayState {
			get => _canChangePlayState;
			set {
				_canChangePlayState = value;
				IsPlaying = _canChangePlayState && PlayerMap["Play"].IsPressed();
			}
		}

		private bool CanPlayNotes => _isPlaying;

		private void HandleMove(InputAction.CallbackContext context) {
			if (!CanMove) {
				return;
			}

			var input = Vector2Int.RoundToInt(context.ReadValue<Vector2>());
			var delta = Vector2Int.zero;

			if (input.x != 0 && _lastInput.x == 0) {
				delta.x = input.x;
			}

			if (input.y != 0 && _lastInput.y == 0) {
				delta.y = input.y;
			}

			_lastInput = input;

			if (delta != Vector2Int.zero) {
				gameObject.BroadcastMessage(nameof(IAvatarMessages.OnMove), delta, SendMessageOptions.DontRequireReceiver);
			}
		}

		[SerializeField, ReadOnly]
		private bool _isPlaying = false;

		private bool IsPlaying {
			get => _isPlaying;
			set {
				if (_isPlaying != value) {
					_isPlaying = value;
					if (value) {
						gameObject.BroadcastMessage(nameof(INoteMessages.OnStartPlaying), SendMessageOptions.DontRequireReceiver);
					} else {
						gameObject.BroadcastMessage(nameof(INoteMessages.OnStopPlaying), SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}

		public void HandlePlayStart(InputAction.CallbackContext context) {
			if (!CanChangePlayState) {
				return;
			}

			IsPlaying = true;
		}

		public void HandlePlayStop(InputAction.CallbackContext context) {
			if (!CanChangePlayState) {
				return;
			}

			IsPlaying = false;
		}

		private InputActionMap NoteMap => playerActions.FindActionMap("Notes");

		private void SetUpNotes() {
			foreach (var noteAction in NoteMap) {
				noteAction.started += HandleNoteStart;
				noteAction.canceled += HandleNoteStop;
			}

			NoteMap.Enable();
		}

		private void TearDownNotes() {
			NoteMap.Disable();
			foreach (var noteAction in NoteMap) {
				noteAction.started -= HandleNoteStart;
				noteAction.canceled -= HandleNoteStop;
			}
		}

		private void HandleNoteStart(InputAction.CallbackContext context) {
			if (!CanPlayNotes) {
				return;
			}

			if (TryLookupNote(context.action, out var note)) {
				StartNote(note);
			}
		}

		private void HandleNoteStop(InputAction.CallbackContext context) {
			if (!CanPlayNotes) {
				return;
			}

			if (TryLookupNote(context.action, out var note)) {
				StopNote(note);
			}
		}

		[Header("Notes")]
		[SerializeField]
		private string _noteLabel = "note";

		[SerializeField, ReadOnly]
		private List<NoteAsset> _notes = new();

		private void AddNote(NoteAsset note) {
			_notes.Add(note);
		}

		private IEnumerator LoadNotes() {
			var locationHandle = Addressables.LoadResourceLocationsAsync(_noteLabel);
			yield return locationHandle;

			yield return Addressables.LoadAssetsAsync<NoteAsset>(locationHandle.Result, AddNote);
		}

		private IEnumerator Start() {
			yield return LoadNotes();
		}

		private bool TryLookupNote(InputAction action, out NoteAsset note) {
			for (var i = 0; i < _notes.Count; i++) {
				if (_notes[i].Is(action)) {
					note = _notes[i];
					return true;
				}
			}

			note = null;
			return false;
		}

		public void StartNote(NoteAsset note) {
			gameObject.BroadcastMessage(nameof(INoteMessages.OnStartNote), note, SendMessageOptions.DontRequireReceiver);
		}

		public void StopNote(NoteAsset note) {
			gameObject.BroadcastMessage(nameof(INoteMessages.OnStopNote), note, SendMessageOptions.DontRequireReceiver);
		}

		public void PlayNoteOneShot(NoteAsset note) {
			if (!CanPlayNotes) {
				return;
			}

			IEnumerator Stop() {
				yield return Wait.forSeconds[0.1f];
				StopNote(note);
			}

			StartNote(note);
			_ = StartCoroutine(Stop());
		}
	}
}
