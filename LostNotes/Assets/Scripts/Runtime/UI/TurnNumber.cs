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
			if (_ourTurnActor?.TurnOrder == null)
				return;

			var turnOrder = _ourTurnActor.TurnOrder;

			if (turnOrder.RoundDone || !_ourTurnActor.HasTurnActions())
				_text.text = "";
			else {
				var turnDistance = turnOrder.GetTurnOrderDistance(_ourTurnActor);
				_text.text = turnDistance switch {
					-1 => "",
					0  => "↓",
					_  => turnDistance.ToString()
				};
			}
		}

		private void OnValidate() {
			if (!_text)
				_text = GetComponent<TMP_Text>();

			_ourTurnActor ??= GetComponentInParent<ITurnActor>();
		}
	}
}
