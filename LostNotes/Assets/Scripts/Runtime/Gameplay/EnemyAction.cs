using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay {
	public abstract class EnemyAction : ScriptableObject {
		public abstract IEnumerator Execute();
	}
}
