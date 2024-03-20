using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Move", menuName = "EnemyActions/Move", order = 0)]
	public class MoveAction : EnemyAction {
		[SerializeField] private Vector2Int _movement;
		[SerializeField] private RotationTurn _turnOnBlocked = RotationTurn.Degrees90;
		[SerializeField] private bool _singleSteps = true;

		public override IEnumerator Execute(Enemy enemy) {
			if (_singleSteps) {
				// X movement
				var xStep = _movement.x < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.x); i++) {
					if (!enemy.Movement.MoveByLocal(new Vector2Int(xStep, 0))) {
						enemy.Movement.Rotate(_turnOnBlocked);
						yield break;
					}

					if (i + 1 < Mathf.Abs(_movement.x))
						yield return new WaitForSeconds(0.5f); // TODO: wait until movement/animation completion
				}

				// Y movement
				var yStep = _movement.y < 0 ? -1 : 1;
				for (var i = 0; i < Mathf.Abs(_movement.y); i++) {
					// TODO: should use ABS as iteration bounds and sign for steps
					if (!enemy.Movement.MoveByLocal(new Vector2Int(0, yStep))) {
						enemy.Movement.Rotate(_turnOnBlocked);
						yield break;
					}

					if (i + 1 < Mathf.Abs(_movement.y))
						yield return new WaitForSeconds(0.5f); // TODO: wait until movement/animation completion
				}
			} else {
				if (!enemy.Movement.MoveByLocal(_movement))
					enemy.Movement.Rotate(_turnOnBlocked);

				yield return new WaitForSeconds(0.5f); // TODO: wait until movement/animation completion
			}
		}
	}
}
