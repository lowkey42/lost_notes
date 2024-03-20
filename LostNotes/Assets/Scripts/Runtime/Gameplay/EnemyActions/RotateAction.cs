using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Rotate", menuName = "EnemyActions/Rotate", order = 0)]
	internal sealed class RotateAction : EnemyAction {
		[SerializeField]
		private RotationTurn _turns = RotationTurn.Degrees90;

		public override IEnumerator Execute(Enemy enemy) {
			enemy.LevelGridTransform.Rotate(_turns);
			yield return new WaitForSeconds(1); // TODO: wait until movement/animation completion
		}

		public override void CreateTurnIndicators(Enemy enemy, Transform parent) {
			// TODO
		}
	}
}
