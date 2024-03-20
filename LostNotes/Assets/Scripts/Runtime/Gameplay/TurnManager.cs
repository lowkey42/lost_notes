using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class TurnManager : MonoBehaviour {
		public delegate void TurnOrderHandler(TurnOrder turnOrder);

		[SerializeField]
		private GameObject _turnActorsRoot;

		private TurnOrder _currentRoundTurnOrder = null;
		private Coroutine _round;

		public TurnOrder CurrentRoundTurnOrder => _currentRoundTurnOrder ??= ComputeTurnOrder();

		private void Update() {
			_round ??= StartCoroutine(DoRound());
		}

		public event TurnOrderHandler OnNewRound = delegate { };
		public event TurnOrderHandler OnNewTurn = delegate { };

		private TurnOrder ComputeTurnOrder() {
			var actors = new List<ITurnActor>();
			_turnActorsRoot.GetComponentsInChildren(actors);
			_ = actors.RemoveAll(actor => !actor.HasTurnActions());
			return new TurnOrder(actors);
		}

		private IEnumerator DoRound() {
			var turnOrder = CurrentRoundTurnOrder;

			OnNewRound(turnOrder);

			for (var i = 0; i < turnOrder.Actors.Count; ++i) {
				turnOrder.CurrentActor = i;
				turnOrder.Actors[i].gameObject.BroadcastMessage(nameof(IActorMessages.OnStartTurn), turnOrder, SendMessageOptions.DontRequireReceiver);
				OnNewTurn(turnOrder);
				yield return turnOrder.Actors[i].DoTurn();
				turnOrder.Actors[i].gameObject.BroadcastMessage(nameof(IActorMessages.OnEndTurn), SendMessageOptions.DontRequireReceiver);
			}

			turnOrder.CurrentActor = turnOrder.Actors.Count;
			_round = null;
		}
	}
}
