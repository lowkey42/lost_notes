using LostNotes.Gameplay;
using TMPro;
using UnityEngine;

namespace LostNotes.UI {
	[RequireComponent(typeof(TMP_Text))]
	public class TurnNumber : MonoBehaviour {
		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private TurnManager _turnManager;

		[SerializeField]
		private ITurnActor _ourTurnActor;

		private TurnOrder _round;

		private void Start() {
			OnValidate();
			if (_turnManager)
				_turnManager.OnNewRound += OnNewRound;
		}

		private void Update() {
			if (_round == null || _ourTurnActor == null)
				return;

			if (_round.RoundDone || !_ourTurnActor.HasTurnActions())
				_text.text = "";
			else {
				var index = _round.Actors.IndexOf(_ourTurnActor);
				_text.text = index == -1 ? "" : ComputeTurnPosition(_round, index);
			}
		}

		private void OnDestroy() {
			if (_turnManager)
				_turnManager.OnNewRound -= OnNewRound;
		}

		private void OnValidate() {
			if (!_text)
				_text = GetComponent<TMP_Text>();

			if (!_turnManager)
				_turnManager = GetComponentInParent<TurnManager>();

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

		private void OnNewRound(TurnOrder round) {
			_round = round;
		}
	}
}
