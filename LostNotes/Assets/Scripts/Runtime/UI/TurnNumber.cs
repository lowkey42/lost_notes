using LostNotes.Gameplay;
using TMPro;
using UnityEngine;

namespace LostNotes.UI {
	[RequireComponent(typeof(TMP_Text))]
	public class TurnNumber : MonoBehaviour {
		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private ITurnActor _ourTurnActor;

		private void Start() {
			OnValidate();
		}

		private void Update() {
			if (_ourTurnActor == null || _ourTurnActor.TurnOrder == null)
				return;

			var turnOrder = _ourTurnActor.TurnOrder;

			if (turnOrder.RoundDone || !_ourTurnActor.HasTurnActions())
				_text.text = "";
			else {
				var index = turnOrder.Actors.IndexOf(_ourTurnActor);
				_text.text = index == -1 ? "" : ComputeTurnPosition(turnOrder, index);
			}
		}

		private void OnValidate() {
			if (!_text)
				_text = GetComponent<TMP_Text>();

			_ourTurnActor ??= GetComponentInParent<ITurnActor>();
		}

		private static string ComputeTurnPosition(TurnOrder round, int index) {
			if (round.CurrentActor >= round.Actors.Count)
				return "";

			var turnPosition = 0;
			for (var i = round.CurrentActor; i != index; i = (i + 1) % round.Actors.Count) {
				if (round.Actors[i].HasTurnActions())
					turnPosition++;
			}

			return turnPosition == 0 ? "↓" : turnPosition.ToString();
		}
	}
}
