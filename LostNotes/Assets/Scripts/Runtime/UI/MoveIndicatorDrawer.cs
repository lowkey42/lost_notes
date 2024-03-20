using LostNotes.Gameplay;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.UI {
	internal sealed class MoveIndicatorDrawer : MonoBehaviour, IActorMessages {
		[SerializeField]
		[Expandable]
		private GameObjectEventChannel moveChannel;

		private ITurnActor _actor;

		private bool _ourTurn = false;

		private GameObject _turnIndicatorRoot;

		private void Start() {
			_actor = GetComponentInParent<ITurnActor>();
			RecreateIndicators();
		}

		private void OnEnable() {
			moveChannel.onTrigger += HandleMove;
		}

		private void OnDisable() {
			moveChannel.onTrigger -= HandleMove;
		}

		public void OnStartTurn(TurnOrder round) {
			_ourTurn = true;
		}

		public void OnEndTurn() {
			_ourTurn = false;
			RecreateIndicators();
		}

		private void HandleMove(GameObject obj) {
			RecreateIndicators();
		}

		private void ClearIndicators() {
			if (_turnIndicatorRoot) Destroy(_turnIndicatorRoot);
		}

		private void RecreateIndicators() {
			if (_actor == null || _ourTurn)
				return;

			ClearIndicators();
			_turnIndicatorRoot = new GameObject("Indicators");
			_actor.CreateTurnIndicators(_turnIndicatorRoot.transform);
		}
	}
}
