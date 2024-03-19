using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	[CreateAssetMenu(fileName = "Move", menuName = "EnemyActions/Move", order = 0)]
	public class MoveAction : EnemyAction {
		[SerializeField] private Vector2Int _movement;

		public override IEnumerator Execute() {
			return null; // TODO: send move command to movement-script and wait until completion
		}
	}
}
