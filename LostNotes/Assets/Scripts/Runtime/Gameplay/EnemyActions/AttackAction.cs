using System.Collections;
using System.Collections.Generic;
using LostNotes.Level;
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
		private float _attackSpeed = 1.0f;

		[SerializeField]
		private float _endTurnDelay = 0.25f;

		[SerializeField]
		private float _recoilDelay = 0.05f;

		[SerializeField]
		private float _recoilDurationFactor = 0.25f;

		[SerializeField]
		private float _recoilJumpHeight = 0.5f;

		[SerializeField]
		private GameObject _projectilePrefab;

		[SerializeField]
		private List<Vector2Int> _projectileTargetPositions = new();

		[SerializeField]
		private float _initialDelay = 0.1f;

		public override IEnumerator Execute(Enemy enemy) {
			enemy.BroadcastMessage(nameof(IEnemyMessages.OnStartAttack), _attackSpeed, SendMessageOptions.DontRequireReceiver);

			if (_initialDelay > 0)
				yield return new WaitForSeconds(_initialDelay);
			
			enemy.LevelGridTransform.SendMessageToObjectsInArea(_attackArea, nameof(IAttackMessages.OnAttacked));

			if (_projectileTargetPositions.Count > 0 && _projectilePrefab) {
				var startPosition = enemy.transform.position;
				foreach (var target in _projectileTargetPositions) {
					var p = Instantiate(_projectilePrefab, startPosition, Quaternion.identity, enemy.transform.parent).GetComponent<Projectile>();
					p.PushOnStart = enemy.LevelGridTransform.LocalToWorldPosition(target);
					p.PushDurationFactor = _recoilDurationFactor;
				}
			}
			
			if (_recoil.x != 0 || _recoil.y != 0) {
				if (_recoilDelay > 0)
					yield return new WaitForSeconds(_recoilDelay);

				var now = Time.time;
				yield return enemy.LevelGridTransform.MoveByLocal(_recoil, _recoilJumpHeight, _recoilDurationFactor);
				var delayLeft = _endTurnDelay - (Time.time - now);
				if (delayLeft > 0)
					yield return new WaitForSeconds(delayLeft);
			} else
				yield return new WaitForSeconds(_endTurnDelay);

			enemy.BroadcastMessage(nameof(IEnemyMessages.OnEndAttack), SendMessageOptions.DontRequireReceiver);
		}

		public override void CreateTurnIndicators(FutureEnemyState enemy, Transform parent) {
			if (!_indicatorPrefab)
				return;

			foreach (var tilePosition in enemy.Level.TilesInArea(enemy.Position2d, enemy.Rotation2d, _attackArea))
				Instantiate(_indicatorPrefab, enemy.Level.GridToWorld(tilePosition), Quaternion.identity, parent);

			if ((_recoil.x != 0 || _recoil.y != 0) && _recoilIndicatorPrefab && enemy.CanMoveBy(_recoil))
				Instantiate(_recoilIndicatorPrefab, enemy.Level.GridToWorld(enemy.LocalToWorldPosition(_recoil)), Quaternion.identity, parent);
		}
	}
}
