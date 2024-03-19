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
			SetUpNotes();
		}

		private void OnDisable() {
			TearDownNotes();
		}

		public void OnMove(InputValue value) {
			var move = Vector2Int.RoundToInt(value.Get<Vector2>());
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

		private InputActionMap noteMap => playerActions.FindActionMap("Notes");

		[SerializeField, ReadOnly]
		private bool _isPlaying = false;

		public void OnPlay(InputValue value) {
			if (value.isPressed) {
				_isPlaying = true;
				noteMap.Enable();
				gameObject.SendMessage(nameof(INoteMessages.StartPlaying), SendMessageOptions.DontRequireReceiver);
			} else {
				_isPlaying = false;
				noteMap.Disable();
				gameObject.SendMessage(nameof(INoteMessages.StopPlaying), SendMessageOptions.DontRequireReceiver);
			}
		}

		private void SetUpNotes() {
			foreach (var noteAction in noteMap) {
				noteAction.started += StartPlayingNote;
				noteAction.canceled += StopPlayingNote;
			}
		}

		private void TearDownNotes() {
			foreach (var noteAction in noteMap) {
				noteAction.started -= StartPlayingNote;
				noteAction.canceled -= StopPlayingNote;
			}
		}

		private void StartPlayingNote(InputAction.CallbackContext context) {
			if (_isPlaying) {
				gameObject.SendMessage(nameof(INoteMessages.StartNote), context.action, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void StopPlayingNote(InputAction.CallbackContext context) {
			if (_isPlaying) {
				gameObject.SendMessage(nameof(INoteMessages.StopNote), context.action, SendMessageOptions.DontRequireReceiver);
			}
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
