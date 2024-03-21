using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Move", menuName = "EnemyActions/Move", order = 0)]
	internal sealed class MoveAction : EnemyAction {
		[SerializeField]
		private GameObject _moveIndicatorPrefab;

		[SerializeField]
		private GameObject _turnIndicatorPrefab;

		[SerializeField]
		private GameObject _stopIndicatorPrefab;

		[SerializeField]
		private Vector2Int _movement;

		[SerializeField]
		private RotationTurn _turnOnBlocked = RotationTurn.Degrees90;

		[SerializeField]
		private bool _singleSteps = true;

		[SerializeField]
		private float _interpolatedDurationFactor = 1.0f;

		[SerializeField]
		private float _jumpHeight = 0.0f;

		public override IEnumerator Execute(Enemy enemy) {
			if (_singleSteps) {
				// X movement
				var xStep = _movement.x < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.x); i++) {
					var move = enemy.LevelGridTransform.MoveByLocal(new Vector2Int(xStep, 0), _jumpHeight, _interpolatedDurationFactor);
					if (move == null) {
						enemy.LevelGridTransform.Rotate(_turnOnBlocked);
						yield break;
					}

					yield return move;
				}

				// Y movement
				var yStep = _movement.y < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.y); i++) {
					var move = enemy.LevelGridTransform.MoveByLocal(new Vector2Int(0, yStep), _jumpHeight, _interpolatedDurationFactor);
					if (move == null) {
						enemy.LevelGridTransform.Rotate(_turnOnBlocked);
						yield break;
					}

					yield return move;
				}
			} else {
				var move = enemy.LevelGridTransform.MoveByLocal(_movement, _jumpHeight, _interpolatedDurationFactor);
				if (move == null)
					enemy.LevelGridTransform.Rotate(_turnOnBlocked);
				else
					yield return move;
			}
		}

		public override void CreateTurnIndicators(FutureEnemyState enemy, Transform parent) {
			if (!_moveIndicatorPrefab || !_turnIndicatorPrefab || !_stopIndicatorPrefab)
				return;

			if (!CreateStepIndicators(new Vector2Int(_movement.x < 0 ? -1 : 1, 0), Mathf.Abs(_movement.x), _movement.x < 0 ? 180 : 0)) return;

			if (!CreateStepIndicators(new Vector2Int(0, _movement.y < 0 ? -1 : 1), Mathf.Abs(_movement.y), _movement.y < 0 ? 90 : -90))
				return;

			Instantiate(_stopIndicatorPrefab, enemy.Position3d, Quaternion.identity, parent);
			return;

			bool CreateStepIndicators(Vector2Int step, int stepCount, int indicatorRotation) {
				var worldStep = enemy.LocalToWorldVector(step);
				var orientation = Quaternion.Euler(0, enemy.Rotation2d + indicatorRotation, 0);

				Quaternion.FromToRotation(enemy.Position3d, enemy.Level.GridToWorld(enemy.Position2d + worldStep));
				for (var i = 0; i < stepCount; i++) {
					var newPosition = enemy.Position2d + worldStep;
					if (enemy.CanMoveTo(newPosition)) {
						if (_singleSteps)
							Instantiate(_moveIndicatorPrefab, enemy.Position3d, orientation, parent);
					} else {
						Instantiate(_turnIndicatorPrefab, enemy.Position3d, orientation, parent);
						return false;
					}

					enemy.Position2d = newPosition;
				}

				return true;
			}
		}
	}
}
