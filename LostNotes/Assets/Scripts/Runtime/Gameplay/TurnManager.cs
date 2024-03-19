using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostNotes.Gameplay {
	public class TurnOrder {
		public TurnOrder(IReadOnlyList<ITurnActor> actors) {
			Actors = actors;
			CurrentActor = 0;
		}

		public IReadOnlyList<ITurnActor> Actors { get; }
		public int CurrentActor { get; set; }

		public bool RoundDone => CurrentActor >= Actors.Count;
	}

	public class TurnManager : MonoBehaviour {
		[SerializeField] private GameObject _turnActorsRoot;

		private TurnOrder _currentRoundTurnOrder = null;
		private Coroutine _round;

		public TurnOrder CurrentRoundTurnOrder => _currentRoundTurnOrder ??= ComputeTurnOrder();

		protected void Update() {
			_round ??= StartCoroutine(DoRound());
		}

		private TurnOrder ComputeTurnOrder() {
			var actors = new List<ITurnActor>();
			_turnActorsRoot.GetComponentsInChildren(actors);
			_ = actors.RemoveAll(actor => !actor.HasTurnActions());
			return new TurnOrder(actors);
		}

		private IEnumerator DoRound() {
			var turnOrder = CurrentRoundTurnOrder;

			for (var i = 0; i < turnOrder.Actors.Count; ++i) {
				turnOrder.CurrentActor = i;
				yield return turnOrder.Actors[i].DoTurn();
			}

			turnOrder.CurrentActor = turnOrder.Actors.Count;
			_round = null;
		}
	}
}
