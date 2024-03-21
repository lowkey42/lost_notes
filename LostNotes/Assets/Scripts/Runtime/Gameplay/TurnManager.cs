using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Gameplay {
	internal sealed class TurnManager : MonoBehaviour {
		public delegate void TurnOrderHandler(TurnOrder turnOrder);

		[SerializeField]
		private Tilemap _turnActorsRoot;

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
			var meta = actors.ToDictionary(actor => actor, actor => _turnActorsRoot.GetColor(_turnActorsRoot.WorldToCell(actor.gameObject.transform.position)));
			actors.Sort(new ActorComparer(meta));
			return new TurnOrder(actors);
		}

		private IEnumerator DoRound() {
			var turnOrder = CurrentRoundTurnOrder;

			foreach (var a in turnOrder.Actors)
				a.TurnOrder = turnOrder;

			OnNewRound(turnOrder);

			for (var i = 0; i < turnOrder.Actors.Count; ++i) {
				turnOrder.CurrentActor = i;
				turnOrder.Actors[i].gameObject.BroadcastMessage(nameof(IActorMessages.OnStartTurn), turnOrder, SendMessageOptions.DontRequireReceiver);
				foreach (var a in turnOrder.Actors)
					a.gameObject.BroadcastMessage(nameof(IActorMessages.OnStartAnyTurn), turnOrder, SendMessageOptions.DontRequireReceiver);
				
				OnNewTurn(turnOrder);
				yield return turnOrder.Actors[i].DoTurn();
				turnOrder.Actors[i].gameObject.BroadcastMessage(nameof(IActorMessages.OnEndTurn), SendMessageOptions.DontRequireReceiver);
			}

			turnOrder.CurrentActor = turnOrder.Actors.Count;
			_round = null;
		}

		private sealed class ActorComparer : IComparer<ITurnActor> {
			private readonly IReadOnlyDictionary<ITurnActor, Color> _meta;

			public ActorComparer(IReadOnlyDictionary<ITurnActor, Color> meta) {
				_meta = meta;
			}

			public int Compare(ITurnActor x, ITurnActor y) {
				return _meta[x].r.CompareTo(_meta[y].r);
			}
		}
	}
}
