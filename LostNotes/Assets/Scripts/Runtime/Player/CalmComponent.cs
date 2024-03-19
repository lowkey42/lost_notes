using System.Collections;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class CalmComponent : MonoBehaviour {
		private IEnumerator Start() {
			Debug.Log("Calming enemies...");
			yield return Wait.forSeconds[1];
		}
	}
}
