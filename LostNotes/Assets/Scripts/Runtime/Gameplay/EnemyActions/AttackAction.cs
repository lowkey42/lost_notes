using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Attack", menuName = "EnemyActions/Attack", order = 0)]
	internal sealed class AttackAction : EnemyAction {
		[SerializeField]
		private GameObject _indicatorPrefab;

		[SerializeField]
		private Vector2Int _recoil;

		[SerializeField]
		private TilemapMask _attackArea = new(new Vector2Int(9, 9));

		public override IEnumerator Execute(Enemy enemy) {
			enemy.LevelGridTransform.SendMessageToObjectsInArea(_attackArea, nameof(IAttackMessages.OnAttacked));

			_ = enemy.LevelGridTransform.MoveByLocal(_recoil);

			return null; // TODO: play animation => wait until completion
		}

		public override void CreateTurnIndicators(Enemy enemy, Transform parent) {
			// TODO
		}
	}
}
