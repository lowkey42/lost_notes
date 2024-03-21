using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Attack", menuName = "EnemyActions/Attack", order = 0)]
	internal sealed class AttackAction : EnemyAction {
		[SerializeField]
		private GameObject _indicatorPrefab;

		[SerializeField]
		private GameObject _recoilIndicatorPrefab;

		[SerializeField]
		private Vector2Int _recoil;

		[SerializeField]
		private TilemapMask _attackArea = new(new Vector2Int(9, 9));

		[SerializeField]
		private float _recoilDelay = 0.05f;

		[SerializeField]
		private float _recoilDurationFactor = 0.25f;

		[SerializeField]
		private float _recoilJumpHeight = 0.5f;

		public override IEnumerator Execute(Enemy enemy) {
			enemy.LevelGridTransform.SendMessageToObjectsInArea(_attackArea, nameof(IAttackMessages.OnAttacked));

			if (_recoil.x != 0 || _recoil.y != 0) {
				if (_recoilDelay > 0)
					yield return new WaitForSeconds(_recoilDelay);

				yield return enemy.LevelGridTransform.MoveByLocal(_recoil, _recoilJumpHeight, _recoilDurationFactor);
			}

			// TODO: play animation => wait until completion
		}

		public override void CreateTurnIndicators(FutureEnemyState enemy, Transform parent) {
			if (!_indicatorPrefab)
				return;

			foreach (var tilePosition in enemy.Level.TilesInArea(enemy.Position2d, enemy.Rotation2d, _attackArea))
				Instantiate(_indicatorPrefab, enemy.Level.GridToWorld(tilePosition), Quaternion.identity, parent);

			if ((_recoil.x != 0 || _recoil.y != 0) && _recoilIndicatorPrefab && enemy.CanMoveBy(_recoil))
				Instantiate(_indicatorPrefab, enemy.Level.GridToWorld(enemy.LocalToWorldPosition(_recoil)), Quaternion.identity, parent);
		}
	}
}
