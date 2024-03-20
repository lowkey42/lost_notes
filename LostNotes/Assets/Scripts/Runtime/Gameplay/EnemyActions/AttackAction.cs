using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Attack", menuName = "EnemyActions/Attack", order = 0)]
	internal sealed class AttackAction : EnemyAction {
		[SerializeField] private Vector2Int _recoil;

		[SerializeField] private TilemapMask _attackArea = new(new Vector2Int(9, 9));

		public override IEnumerator Execute(Enemy enemy) {
			// TODO:
			// - iterate over _attackArea
			// - transform each position to local space
			// - Check tile position => if IAttackable, call OnAttacked

			_ = enemy.Movement.MoveByLocal(_recoil);

			return null; // TODO: play animation => wait until completion
		}
	}
}
