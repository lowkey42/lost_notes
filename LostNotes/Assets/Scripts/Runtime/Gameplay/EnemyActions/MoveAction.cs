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

		public override IEnumerator Execute(Enemy enemy) {
			if (_singleSteps) {
				// X movement
				var xStep = _movement.x < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.x); i++) {
					if (!enemy.LevelGridTransform.MoveByLocal(new Vector2Int(xStep, 0))) {
						enemy.LevelGridTransform.Rotate(_turnOnBlocked);
						yield break;
					}

					if (i + 1 < Mathf.Abs(_movement.x))
						yield return new WaitForSeconds(0.5f); // TODO: wait until movement/animation completion
				}

				// Y movement
				var yStep = _movement.y < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.y); i++) {
					// TODO: should use ABS as iteration bounds and sign for steps
					if (!enemy.LevelGridTransform.MoveByLocal(new Vector2Int(0, yStep))) {
						enemy.LevelGridTransform.Rotate(_turnOnBlocked);
						yield break;
					}

					if (i + 1 < Mathf.Abs(_movement.y))
						yield return new WaitForSeconds(0.5f); // TODO: wait until movement/animation completion
				}
			} else {
				if (!enemy.LevelGridTransform.MoveByLocal(_movement))
					enemy.LevelGridTransform.Rotate(_turnOnBlocked);

				yield return new WaitForSeconds(0.5f); // TODO: wait until movement/animation completion
			}
		}

		public override void CreateTurnIndicators(Enemy enemy, Transform parent) {
			if (!_moveIndicatorPrefab || !_turnIndicatorPrefab || !_stopIndicatorPrefab)
				return;

			var gridTransform = enemy.LevelGridTransform;
			var position = gridTransform.Position2d;

			if (!CreateStepIndicators(new Vector2Int(_movement.x < 0 ? -1 : 1, 0), Mathf.Abs(_movement.x), _movement.x < 0 ? 180 : 0)) return;

			if (!CreateStepIndicators(new Vector2Int(0, _movement.y < 0 ? -1 : 1), Mathf.Abs(_movement.y), _movement.y < 0 ? 90 : -90))
				return;

			Instantiate(_stopIndicatorPrefab, gridTransform.GridTo3dPosition(position), Quaternion.identity, parent);
			return;

			bool CreateStepIndicators(Vector2Int step, int stepCount, int indicatorRotation) {
				var worldStep = gridTransform.LocalToWorldVector(step);
				var orientation = Quaternion.Euler(0, gridTransform.Rotation2d + indicatorRotation, 0);

				Quaternion.FromToRotation(gridTransform.GridTo3dPosition(position), gridTransform.GridTo3dPosition(position + worldStep));
				for (var i = 0; i < stepCount; i++) {
					var newPosition = position + worldStep;
					if (gridTransform.CanMoveTo(newPosition))
						Instantiate(_moveIndicatorPrefab, gridTransform.GridTo3dPosition(position), orientation, parent);
					else {
						Instantiate(_turnIndicatorPrefab, gridTransform.GridTo3dPosition(position), orientation, parent);
						return false;
					}

					position = newPosition;
				}

				return true;
			}
		}
	}
}
