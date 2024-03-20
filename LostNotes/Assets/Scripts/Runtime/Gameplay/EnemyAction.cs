using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal abstract class EnemyAction : ScriptableObject {
		public abstract IEnumerator Execute(Enemy enemy);

		public abstract void CreateTurnIndicators(FutureEnemyState enemy, Transform parent);
	}
}
