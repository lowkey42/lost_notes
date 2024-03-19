using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	public class AttackAction : EnemyAction {

		[SerializeField] private Vector2Int _recoil;
		// TODO: Tilemap pattern
		
		public override IEnumerator Execute() {
			return null; // TODO: play animation, damage attack-able objects in pattern + send move command to movement-script => wait until completion
		}
	}
}
