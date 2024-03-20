using System.Collections;
using LostNotes.Player;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostNotes.Gameplay {
	internal sealed class AvatarActor : MonoBehaviour, ITurnActor, IAvatarMessages, IAttackMessages {
		[SerializeField, Range(0, 10)]
		private int _actionPointsPerTurn = 3;
		[SerializeField]
		private bool _allowActionsWithoutSufficientPoints = true;
		[SerializeField, Range(0, 10)]
		private int _actionPointsToMove = 1;
		[SerializeField, Range(0, 10)]
		private int _actionPointsToPlay = 2;
		private int _actionPoints = 0;
		private int ActionPoints {
			get => _actionPoints;
			set {
				_actionPoints = value;

				if (_allowActionsWithoutSufficientPoints) {
					_input.CanMove = _actionPoints > 0;
					_input.CanChangePlayState = _actionPoints > 0;
				} else {
					_input.CanMove = _actionPoints >= _actionPointsToMove;
					_input.CanChangePlayState = _actionPoints >= _actionPointsToPlay;
				}
			}
		}

		[SerializeField]
		private AvatarInput _input;

		private void Awake() {
			_input.CanMove = false;
			_input.CanChangePlayState = false;
		}

		public IEnumerator DoTurn() {
			if (_isAlive) {
				ActionPoints = _actionPointsPerTurn;
				do {
					yield return null;
				} while (_input.CanMove || _input.CanChangePlayState);
			}

			if (!_isAlive) {
				Debug.LogWarning("I is ded", this);
				yield return Wait.forSeconds[1];
				gameObject.BroadcastMessage(nameof(IAvatarMessages.OnReset), SendMessageOptions.DontRequireReceiver);
				yield break;
			}
		}

		public void OnMove(Vector2Int delta) {
			ActionPoints -= _actionPointsToMove;
		}

		public void OnPlaySong(SongAsset song) {
			ActionPoints -= _actionPointsToPlay;
		}

		public void OnFailSong(SongAsset song) {
			ActionPoints -= _actionPointsToPlay;
		}

		[SerializeField, ReadOnly]
		private bool _isAlive = true;

		[ContextMenu(nameof(OnAttacked))]
		public void OnAttacked() {
			_isAlive = false;
			ActionPoints = 0;
		}

		public void OnReset() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
