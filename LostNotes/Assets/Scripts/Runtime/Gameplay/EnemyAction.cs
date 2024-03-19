using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay {
	[CreateAssetMenu(fileName = "EnemyAction", menuName = "LostNotes/EnemyAction", order = 0)]
	public abstract class EnemyAction : ScriptableObject {
		public abstract IEnumerator Execute();
	}
}
