using System.Collections;
using LostNotes.Player;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class AvatarActor : MonoBehaviour, ITurnActor, IAvatarMessages {
		[SerializeField, Range(0, 10)]
		private int _actionPointsPerTurn = 3;
		[SerializeField, Range(0, 10)]
		private int _actionPointsToMove = 1;
		[SerializeField, Range(0, 10)]
		private int _actionPointsToPlay = 2;
		private int _actionPoints = 0;

		[SerializeField]
		private AvatarInput _input;

		private void Awake() {
			_input.enabled = false;
		}

		public IEnumerator DoTurn() {
			_input.enabled = true;

			_actionPoints = _actionPointsPerTurn;
			while (_actionPoints > 0) {
				yield return null;
			}

			_input.enabled = false;
		}

		public void MoveBy(Vector2Int delta) {
			_actionPoints -= _actionPointsToMove;
		}

		public void PlaySong(SongAsset song) {
			_actionPoints -= _actionPointsToPlay;
		}

		public void FailSong(SongAsset song) {
			_actionPoints -= _actionPointsToPlay;
		}
	}
}
