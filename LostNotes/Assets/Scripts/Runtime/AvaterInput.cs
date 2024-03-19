using System.Collections;
using System.Collections.Generic;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LostNotes {
	internal sealed class AvaterInput : MonoBehaviour {
		[SerializeField]
		private string _songLabel;
		[SerializeField, ReadOnly]
		private List<SongAsset> _songs = new();

		private void AddSong(SongAsset song) {
			_songs.Add(song);
		}

		private IEnumerator LoadSongs() {
			AsyncOperationHandle<IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync(_songLabel);
			yield return locationHandle;

			yield return Addressables.LoadAssetsAsync<SongAsset>(locationHandle.Result, AddSong);
		}

		private Vector2Int Position {
			get => Vector2Int.RoundToInt(transform.position.SwizzleXZ());
			set => transform.position = value.SwizzleXZ();
		}

		private Vector2Int _current;

		private void OnEnable() {
			SetUpNotes();
		}

		private IEnumerator Start() {
			yield return LoadSongs();
		}

		private void OnDisable() {
			TearDownNotes();
		}

		public void OnMove(InputValue value) {
			Vector2Int move = Vector2Int.RoundToInt(value.Get<Vector2>());
			Vector2Int position = Position;

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
		public void OnPlay(InputValue value) {
			if (value.isPressed) {
				noteMap.Enable();
				foreach (SongAsset song in _songs) {
					song.ResetInput();
				}
			} else {
				noteMap.Disable();
			}
		}

		private void SetUpNotes() {
			foreach (InputAction noteAction in noteMap) {
				noteAction.started += StartNote;
				noteAction.canceled += StopNote;
			}
		}

		private void TearDownNotes() {
			foreach (InputAction noteAction in noteMap) {
				noteAction.started -= StartNote;
				noteAction.canceled -= StopNote;
			}
		}

		private void StartNote(InputAction.CallbackContext context) {
			Debug.Log(context.action);
		}

		private void StopNote(InputAction.CallbackContext context) {
			Debug.Log(context.action);
		}
	}
}
