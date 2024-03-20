using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.UI {
	public class MoveIndicatorDrawer : MonoBehaviour {
		[SerializeField]
		private TurnManager _turnManager;

		private GameObject _turnIndicatorRoot;

		private void Start() {
			OnValidate();

			_turnManager.OnNewTurn += OnNewTurn;
			// TODO: register OnTurnOrderChange to fire on OnNewTurn, if it's the beginning of the players turn
		}

		private void OnDestroy() {
			_turnManager.OnNewTurn -= OnNewTurn;
		}

		private void OnValidate() {
			if (!_turnManager)
				_turnManager = GetComponentInParent<TurnManager>();
		}

		private void OnNewTurn(TurnOrder round) {
			if (!round.RoundDone && round.Actors[round.CurrentActor] is AvatarActor)
				RecreateIndicators(round);
			else
				ClearIndicators();
		}

		private void ClearIndicators() {
			Destroy(_turnIndicatorRoot);
		}

		private void RecreateIndicators(TurnOrder round) {
			ClearIndicators();

			_turnIndicatorRoot = new GameObject("Indicators");
			_turnIndicatorRoot.transform.SetParent(transform);

			foreach (var actor in round.Actors) {
				if (actor.HasTurnActions())
					actor.CreateTurnIndicators(_turnIndicatorRoot.transform);
			}
		}
	}
}
