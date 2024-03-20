using LostNotes.Gameplay;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.UI {
	internal sealed class MoveIndicatorDrawer : MonoBehaviour, IActorMessages {
		[SerializeField, Expandable]
		private GameObjectEventChannel moveChannel;

		private GameObject _turnIndicatorRoot;

		private void OnEnable() {
			moveChannel.onTrigger += HandleMove;
		}
		private void OnDisable() {
			moveChannel.onTrigger -= HandleMove;
		}

		private void HandleMove(GameObject obj) {
			if (_round is not null) {
				RecreateIndicators(_round);
			}
		}

		private TurnOrder _round;
		public void OnStartTurn(TurnOrder round) {
			_round = round;
			RecreateIndicators(round);
		}

		public void OnEndTurn() {
			ClearIndicators();
		}

		private void ClearIndicators() {
			if (_turnIndicatorRoot) {
				Destroy(_turnIndicatorRoot);
			}
		}

		private void RecreateIndicators(TurnOrder round) {
			ClearIndicators();

			_turnIndicatorRoot = new GameObject("Indicators");

			foreach (var actor in round.Actors) {
				if (actor.HasTurnActions())
					actor.CreateTurnIndicators(_turnIndicatorRoot.transform);
			}
		}
	}
}
