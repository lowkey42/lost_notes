using System.Collections;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes.Player {
	internal sealed class AvaterInput : MonoBehaviour {

		private Vector2Int Position {
			get => Vector2Int.RoundToInt(transform.position.SwizzleXZ());
			set => transform.position = value.SwizzleXZ();
		}

		private Vector2Int _current;

		private void OnEnable() {
			SetUpPlayer();
			SetUpNotes();
		}

		private void OnDisable() {
			TearDownPlayer();
			TearDownNotes();
		}

		private void HandleMove(InputAction.CallbackContext context) {
			if (_isPlaying) {
				return;
			}

			var move = Vector2Int.RoundToInt(context.ReadValue<Vector2>());
			var position = Position;

			if (move.x != 0 && _current.x == 0) {
				position.x += move.x;
			}

			if (move.y != 0 && _current.y == 0) {
				position.y += move.y;
			}

			_current = move;
			Position = position;
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

		private InputActionMap NoteMap => playerActions.FindActionMap("Notes");

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
