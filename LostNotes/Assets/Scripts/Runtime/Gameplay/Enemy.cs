using System;
using System.Collections;
using System.Collections.Generic;
using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostNotes.Gameplay {
	internal sealed class Enemy : MonoBehaviour, ITurnActor, IAttackMessages {
		[Serializable]
		public class StatusActions {
			public StatusEffects RequiredStatusEffects;
			public StatusEffects ForbiddenStatusEffects;
			public List<EnemyAction> Actions;
		}
		
		[SerializeField]
		private List<EnemyAction> _actions;

		[SerializeField]
		private ActorStatus _status;

		[SerializeField]
		private List<StatusActions> _statusSpecificActionOverrides = new();

		[field: SerializeField] public LevelGridTransform LevelGridTransform { get; private set; }

		public bool IsSleeping => _status && _status.HasStatusEffect(StatusEffects.Sleeping);

		private void OnValidate() {
			if (!LevelGridTransform)
				LevelGridTransform = GetComponentInChildren<LevelGridTransform>();
			if (!_status)
				_status = GetComponentInChildren<ActorStatus>();
		}

		private void Start() {
			OnValidate();
		}

		public void OnAttacked() {
			if (_status)
				_status.ApplyStatusEffect(StatusEffects.Sleeping);
		}

		public TurnOrder TurnOrder { get; set; }

		public IEnumerator DoTurn() {
			if (!HasTurnActions())
				yield break;

			foreach (var action in GetActions()) {
				if (!gameObject.activeInHierarchy || IsSleeping)
					yield break;
				
				yield return action.Execute(this);
			}
		}

		public bool HasTurnActions() {
			if (IsSleeping)
				return false;

			return GetActions().Count != 0;
		}

		public void CreateTurnIndicators(Transform parent) {
			if (!HasTurnActions())
				return;

			var futureState = new FutureEnemyState(LevelGridTransform.Level, this);
			foreach (var action in GetActions())
				action.CreateTurnIndicators(futureState, parent);
		}

		private List<EnemyAction> GetActions() {
			var status = _status.GetStatusFlags();

			foreach (var o in _statusSpecificActionOverrides) {
				if (status.HasFlag(o.RequiredStatusEffects) && (status & o.ForbiddenStatusEffects) == 0)
					return o.Actions;
			}

			return _actions;
		}
	}
}
