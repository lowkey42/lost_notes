using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostNotes.Gameplay {
	public class Enemy : MonoBehaviour, ITurnActor {
		[SerializeField]        private List<EnemyAction> _actions;
		[field: SerializeField] public  Movement          Movement { get; private set; }

		private void Start() {
			if (!Movement)
				Movement = GetComponentInChildren<Movement>();
		}

		public IEnumerator DoTurn() {
			foreach (var action in _actions)
				yield return action.Execute(this);
		}
	}
}
