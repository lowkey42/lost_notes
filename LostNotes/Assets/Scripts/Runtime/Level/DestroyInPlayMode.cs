using UnityEngine;

namespace LostNotes.Level {
	public sealed class DestroyInPlayMode : MonoBehaviour {
		[SerializeField]
		private Component component;

		private void Awake() {
			Destroy(component);
			Destroy(this);
		}
	}
}
