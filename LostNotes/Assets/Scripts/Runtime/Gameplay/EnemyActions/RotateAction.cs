using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay.EnemyActions {
	public class RotateAction : EnemyAction {
		[SerializeField] [Range(1, 4)] private int _quaterTurns = 1;

		public override IEnumerator Execute() {
			return null; // TODO: send rotate command to movement-script and wait until completion
		}
	}
}