using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class Enemy : MonoBehaviour, ITurnActor, IAttackMessages {
		[SerializeField] private List<EnemyAction> _actions;
		[field: SerializeField] public Movement Movement { get; private set; }

		private bool _alive = true;

		private void Start() {
			if (!Movement)
				Movement = GetComponentInChildren<Movement>();
		}

		public void OnAttacked() {
			Debug.Log("Enemy killed");
			_alive = false;
		}

		public IEnumerator DoTurn() {
			if (!_alive)
				yield break;

			foreach (var action in _actions)
				yield return action.Execute(this);
		}

		public bool HasTurnActions() {
			return _alive;
		}
	}
}
