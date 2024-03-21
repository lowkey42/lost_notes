using System;
using UnityEngine;

namespace LostNotes {
	[CreateAssetMenu]
	internal sealed class GameObjectEventChannel : ScriptableObject {
		public event Action<GameObject> OnTrigger;

		public void Raise(GameObject context) {
			OnTrigger?.Invoke(context);
		}
	}
}
