using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Move", menuName = "EnemyActions/Move", order = 0)]
	internal sealed class MoveAction : EnemyAction {
		[SerializeField]
		private GameObject _moveIndicatorPrefab;

		[SerializeField]
		private GameObject[] _turnIndicatorPrefabs;

		[SerializeField]
		private GameObject _stopIndicatorPrefab;

		[SerializeField]
		private Vector2Int _movement;

		[SerializeField]
		private RotationTurn _minTurnOnBlocked = RotationTurn.Degrees90;

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
					if (!enemy.gameObject.activeInHierarchy || enemy.IsSleeping)
						yield break;
					
					var step = new Vector2Int(xStep, 0);
					var move = enemy.LevelGridTransform.MoveByLocal(step, _jumpHeight, _interpolatedDurationFactor);
					if (move == null) {
						TurnOnCollision(step);
						yield break;
					}

					yield return move;
				}

				// Y movement
				var yStep = _movement.y < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.y); i++) {
					if (!enemy.gameObject.activeInHierarchy || enemy.IsSleeping)
						yield break;

					var step = new Vector2Int(0, yStep);
					var move = enemy.LevelGridTransform.MoveByLocal(step, _jumpHeight, _interpolatedDurationFactor);
					if (move == null) {
						TurnOnCollision(step);
						yield break;
					}

					yield return move;
				}
			} else {
				var move = enemy.LevelGridTransform.MoveByLocal(_movement, _jumpHeight, _interpolatedDurationFactor);
				if (move == null)
					TurnOnCollision(_movement);
				else
					yield return move;
			}

			yield break;

			void TurnOnCollision(Vector2Int step) {
				if (_minTurnOnBlocked == RotationTurn.Degrees0)
					return;

				enemy.LevelGridTransform.Rotate(_minTurnOnBlocked);
				for (var turns = (int) _minTurnOnBlocked; !enemy.LevelGridTransform.CanMoveByLocal(step) && turns < 360; turns += 90)
					enemy.LevelGridTransform.Rotate(RotationTurn.Degrees90);
			}
		}

		public override void CreateTurnIndicators(FutureEnemyState enemy, Transform parent) {
			if (!_moveIndicatorPrefab || !_stopIndicatorPrefab)
				return;

			if (!_singleSteps && !enemy.CanMoveBy(_movement)) {
				CreateRotationIndicators(_movement, 0);
				return;
			}
			
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
					if (!_singleSteps || enemy.CanMoveTo(newPosition))
						Instantiate(_moveIndicatorPrefab, enemy.Position3d, orientation, parent);
					else {
						CreateRotationIndicators(step, indicatorRotation);
						return false;
					}

					enemy.Position2d = newPosition;
				}

				return true;
			}

			void CreateRotationIndicators(Vector2Int step, int indicatorRotation) {
				if (_minTurnOnBlocked == RotationTurn.Degrees0)
					return;

				var orientation = Quaternion.Euler(0, enemy.Rotation2d + indicatorRotation, 0);

				// rotate until next MoveByLocal would succeed
				var turnDegrees = (int) _minTurnOnBlocked;
				enemy.Rotation2d += turnDegrees;
				while (!enemy.CanMoveBy(step) && turnDegrees < 360) {
					turnDegrees += 90;
					enemy.Rotation2d += 90;
				}

				if (_turnIndicatorPrefabs != null && turnDegrees / 90 <= _turnIndicatorPrefabs.Length)
					Instantiate(_turnIndicatorPrefabs[(turnDegrees / 90) - 1], enemy.Position3d, orientation, parent);
				else if (_turnIndicatorPrefabs != null && 1 < _turnIndicatorPrefabs.Length)
					Instantiate(_turnIndicatorPrefabs[1], enemy.Position3d, orientation, parent);
			}
		}
	}
}
