using System.Collections;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class AvaterInput : MonoBehaviour {

		private void OnEnable() {
			SetUpPlayer();
			SetUpNotes();
		}

		private void OnDisable() {
			TearDownPlayer();
			TearDownNotes();
		}

		[SerializeField]
		private InputActionAsset playerActions;

		private InputActionMap PlayerMap => playerActions.FindActionMap("Player");

		private void SetUpPlayer() {
			PlayerMap["Move"].performed += HandleMove;
			PlayerMap["Play"].started += HandlePlayStart;
			PlayerMap["Play"].canceled += HandlePlayStop;
			PlayerMap.Enable();
		}

		private void TearDownPlayer() {
			PlayerMap.Disable();
			PlayerMap["Move"].performed -= HandleMove;
			PlayerMap["Play"].started -= HandlePlayStart;
			PlayerMap["Play"].canceled -= HandlePlayStop;
		}

		private Vector2Int _lastInput;

		private void HandleMove(InputAction.CallbackContext context) {
			if (_isPlaying) {
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
				gameObject.SendMessage(nameof(IAvatarMessages.MoveBy), delta, SendMessageOptions.DontRequireReceiver);
			}
		}

		[SerializeField, ReadOnly]
		private bool _isPlaying = false;

		public void HandlePlayStart(InputAction.CallbackContext context) {
			_isPlaying = true;
			gameObject.SendMessage(nameof(INoteMessages.StartPlaying), SendMessageOptions.DontRequireReceiver);
		}

		public void HandlePlayStop(InputAction.CallbackContext context) {
			_isPlaying = false;
			gameObject.SendMessage(nameof(INoteMessages.StopPlaying), SendMessageOptions.DontRequireReceiver);

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
			if (!_isPlaying) {
				return;
			}

			gameObject.SendMessage(nameof(INoteMessages.StartNote), context.action, SendMessageOptions.DontRequireReceiver);
		}

		private void HandleNoteStop(InputAction.CallbackContext context) {
			if (!_isPlaying) {
				return;
			}

			gameObject.SendMessage(nameof(INoteMessages.StopNote), context.action, SendMessageOptions.DontRequireReceiver);
		}

		public void PlayNote(InputActionReference action) {
			IEnumerator Stop() {
				yield return Wait.forSeconds[1];
				gameObject.SendMessage(nameof(INoteMessages.StopNote), action.action, SendMessageOptions.DontRequireReceiver);
			}

			gameObject.SendMessage(nameof(INoteMessages.StartNote), action.action, SendMessageOptions.DontRequireReceiver);
			_ = StartCoroutine(Stop());
		}
	}
}
