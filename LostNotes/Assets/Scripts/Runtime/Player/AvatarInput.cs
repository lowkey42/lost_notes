using System;
using System.Collections;
using System.Collections.Generic;
using LostNotes.Level;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class AvatarInput : MonoBehaviour {
		public static event Action<EViolinState> OnChangePlayState;
		public static event Action<bool> OnChangeCanMove;
		public static event Action<bool> OnChangeCanPlay;

		private void OnEnable() {
			_ = StartCoroutine(OnEnable_Co());
		}

		private IEnumerator OnEnable_Co() {
			yield return new WaitUntil(() => LevelManager.IsReady);

			SetUpPlayer();
			SetUpNotes();
			SetUpInput();
		}

		private void OnDisable() {
			IsPlaying = false;
			TearDownPlayer();
			TearDownNotes();
			TearDownInput();
		}

		[SerializeField]
		private AvatarInputAsset _avatarInput;

		private void SetUpInput() {
			if (_avatarInput) {
				_avatarInput.OnActionInput += HandleActionInput;
				_avatarInput.OnMoveInput += HandleMoveInput;
				_avatarInput.OnTogglePlay += HandleTogglePlay;
				_avatarInput.OnReset += HandleReset;
				_avatarInput.OnSkipTurn += HandleSkip;
				_avatarInput.OnTogglePause += HandlePause;
			}
		}

		private void TearDownInput() {
			if (_avatarInput) {
				_avatarInput.OnActionInput -= HandleActionInput;
				_avatarInput.OnMoveInput -= HandleMoveInput;
				_avatarInput.OnTogglePlay -= HandleTogglePlay;
				_avatarInput.OnReset -= HandleReset;
				_avatarInput.OnSkipTurn -= HandleSkip;
				_avatarInput.OnTogglePause -= HandlePause;
			}
		}

		private void HandleSkip() {
			gameObject.BroadcastMessage(nameof(IAvatarMessages.OnSkip), SendMessageOptions.DontRequireReceiver);
		}

		private void HandleSkip(InputAction.CallbackContext context) {
			HandleSkip();
		}

		private bool _isPaused;

		private bool IsPaused {
			get => _isPaused;
			set {
				_isPaused = value;
				gameObject.BroadcastMessage(nameof(IAvatarMessages.OnPause), value, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void HandlePause() {
			IsPaused = !IsPaused;
		}

		private void HandlePause(InputAction.CallbackContext context) {
			HandlePause();
		}

		private void HandleReset() {
			gameObject.BroadcastMessage(nameof(IAvatarMessages.OnReset), SendMessageOptions.DontRequireReceiver);
		}

		private void HandleActionInput(InputActionReference input) {
			if (TryLookupNote(input, out var note)) {
				PlayNoteOneShot(note);
			}
		}

		private void HandleMoveInput(Vector2Int input) {
			DoMove(input);
			DoMove(Vector2Int.zero);
		}

		[SerializeField]
		private InputActionAsset _playerActions;

		private InputActionMap PlayerMap => _playerActions.FindActionMap("Player");

		private void SetUpPlayer() {
			PlayerMap.Enable();

			PlayerMap["Move"].performed += HandleMove;
			PlayerMap["Move"].canceled += HandleMove;
			PlayerMap["Play"].started += HandlePlayStart;
			PlayerMap["Play"].canceled += HandlePlayStop;
			PlayerMap["Reset"].performed += HandleReset;
			PlayerMap["Skip"].performed += HandleSkip;
			PlayerMap["Pause"].performed += HandlePause;
		}

		private void TearDownPlayer() {
			PlayerMap["Move"].performed -= HandleMove;
			PlayerMap["Move"].canceled -= HandleMove;
			PlayerMap["Play"].started -= HandlePlayStart;
			PlayerMap["Play"].canceled -= HandlePlayStop;
			PlayerMap["Reset"].performed -= HandleReset;
			PlayerMap["Skip"].performed -= HandleSkip;
			PlayerMap["Pause"].performed -= HandlePause;

			PlayerMap.Disable();
		}

		private void HandleReset(InputAction.CallbackContext context) {
			HandleReset();
		}

		private Vector2Int _lastInput;

		[SerializeField, ReadOnly]
		private bool _canMove = true;
		public bool CanMove {
			get => _canMove && !_isPlaying;
			set {
				_canMove = value;
				OnChangeCanMove?.Invoke(CanMove);
			}
		}

		[SerializeField, ReadOnly]
		private bool _canChangePlayState = true;
		public bool CanChangePlayState {
			get => _canChangePlayState;
			set {
				_canChangePlayState = value;
				OnChangeCanPlay?.Invoke(CanChangePlayState);
				IsPlaying = CanChangePlayState && PlayerMap["Play"].IsPressed();
			}
		}

		private void HandleTogglePlay() {
			IsPlaying = !IsPlaying;
		}

		private bool CanPlayNotes => _isPlaying;

		private void HandleMove(InputAction.CallbackContext context) {
			DoMove(Vector2Int.RoundToInt(context.ReadValue<Vector2>()));
		}

		private void DoMove(Vector2Int input) {
			if (!CanMove) {
				return;
			}

			if (IsPaused) {
				return;
			}

			if (CanMove && input.x != 0 && _lastInput.x == 0) {
				gameObject.BroadcastMessage(nameof(IAvatarMessages.OnMove), new Vector2Int(input.x, 0), SendMessageOptions.DontRequireReceiver);
			}

			if (CanMove && input.y != 0 && _lastInput.y == 0) {
				gameObject.BroadcastMessage(nameof(IAvatarMessages.OnMove), new Vector2Int(0, input.y), SendMessageOptions.DontRequireReceiver);
			}

			_lastInput = input;
		}

		[SerializeField, ReadOnly]
		private bool _isPlaying = false;

		private bool IsPlaying {
			get => _isPlaying;
			set {
				_isPlaying = value;

				if (value) {
					gameObject.BroadcastMessage(nameof(INoteMessages.OnStartPlaying), SendMessageOptions.DontRequireReceiver);
				} else {
					gameObject.BroadcastMessage(nameof(INoteMessages.OnStopPlaying), SendMessageOptions.DontRequireReceiver);
				}

				OnChangePlayState?.Invoke(IsPlaying ? EViolinState.Playing : EViolinState.Idle);
			}
		}

		public void HandlePlayStart(InputAction.CallbackContext context) {
			if (!CanChangePlayState) {
				return;
			}

			if (IsPaused) {
				return;
			}

			IsPlaying = true;
		}

		public void HandlePlayStop(InputAction.CallbackContext context) {
			if (!CanChangePlayState) {
				return;
			}

			if (IsPaused) {
				return;
			}

			IsPlaying = false;
		}

		private InputActionMap NoteMap => _playerActions.FindActionMap("Notes");

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

			if (IsPaused) {
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

			if (IsPaused) {
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

		private IEnumerator Start() {
			yield return _noteLabel.LoadAssetsAsync<NoteAsset>(_notes.Add);
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

			if (IsPaused) {
				return;
			}

			IEnumerator Stop() {
				yield return null;
				StopNote(note);
			}

			StartNote(note);
			_ = StartCoroutine(Stop());
		}
	}
}
