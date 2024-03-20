using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Attack", menuName = "EnemyActions/Attack", order = 0)]
	internal sealed class AttackAction : EnemyAction {
		[SerializeField] private Vector2Int _recoil;

		[SerializeField] private TilemapMask _attackArea = new(new Vector2Int(9, 9));

		public override IEnumerator Execute(Enemy enemy) {
			enemy.Movement.SendMessageToObjectsInArea(_attackArea, nameof(IAttackMessages.OnAttacked));

			_ = enemy.Movement.MoveByLocal(_recoil);

			return null; // TODO: play animation => wait until completion
		}
	}
}
