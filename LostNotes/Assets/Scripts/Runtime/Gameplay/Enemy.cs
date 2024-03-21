using System.Collections;
using System.Collections.Generic;
using LostNotes.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostNotes.Gameplay {
	internal sealed class Enemy : MonoBehaviour, ITurnActor, IAttackMessages {
		
		[SerializeField]
		private List<EnemyAction> _actions;

		[SerializeField]
		private ActorStatus _status;

		[field: SerializeField] public LevelGridTransform LevelGridTransform { get; private set; }
		
		private void Start() {
			if (!LevelGridTransform)
				LevelGridTransform = GetComponentInChildren<LevelGridTransform>();
			if (!_status)
				_status = GetComponentInChildren<ActorStatus>();
		}

		public void OnAttacked() {
			if (_status)
				_status.ApplyStatusEffect(StatusEffects.Sleeping);
		}

		public TurnOrder TurnOrder { get; set; }

		public IEnumerator DoTurn() {
			if (!HasTurnActions())
				yield break;

			foreach (var action in _actions)
				yield return action.Execute(this);
		}

		public bool HasTurnActions() {
			return !_status || !_status.HasStatusEffect(StatusEffects.Sleeping);
		}

		public void CreateTurnIndicators(Transform parent) {
			if (!HasTurnActions())
				return;

			var futureState = new FutureEnemyState(LevelGridTransform.Level, this);
			foreach (var action in _actions)
				action.CreateTurnIndicators(futureState, parent);
		}
	}
}
