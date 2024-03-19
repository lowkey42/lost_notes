using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostNotes.Gameplay {
	public class Enemy : MonoBehaviour, ITurnActor {
		[SerializeField] private List<EnemyAction> _actions;

		public IEnumerator DoTurn() {
			foreach (var action in _actions) yield return action.Execute();
		}
	}
}
