using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Attack", menuName = "EnemyActions/Attack", order = 0)]
	public class AttackAction : EnemyAction {
		[SerializeField] private Vector2Int _recoil;

		[SerializeField] private TilemapMask _attackArea = new(new Vector2Int(9, 9));

		public override IEnumerator Execute() {
			return null; // TODO: play animation, damage attack-able objects in pattern + send move command to movement-script => wait until completion
		}
	}
}
