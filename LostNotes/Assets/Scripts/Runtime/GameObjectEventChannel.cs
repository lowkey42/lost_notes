using System;
using UnityEngine;

namespace LostNotes {
	[CreateAssetMenu]
	internal sealed class GameObjectEventChannel : ScriptableObject {
		public Action<GameObject> onTrigger;

		public void Raise(GameObject context) {
			onTrigger?.Invoke(context);
		}
	}
}
