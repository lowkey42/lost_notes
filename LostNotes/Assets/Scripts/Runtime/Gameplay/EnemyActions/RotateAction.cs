using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Rotate", menuName = "EnemyActions/Rotate", order = 0)]
	internal sealed class RotateAction : EnemyAction {
		[SerializeField]
		private GameObject _indicatorPrefab;

		[SerializeField]
		private RotationTurn _turns = RotationTurn.Degrees90;

		public override IEnumerator Execute(Enemy enemy) {
			enemy.LevelGridTransform.Rotate(_turns);
			yield return new WaitForSeconds(0.2f);
		}

		public override void CreateTurnIndicators(FutureEnemyState enemy, Transform parent) {
			if (!_indicatorPrefab)
				return;

			Instantiate(_indicatorPrefab, enemy.Position3d, Quaternion.Euler(0, enemy.Rotation2d, 0), parent);
		}
	}
}
