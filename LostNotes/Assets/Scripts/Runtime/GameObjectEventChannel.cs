using System;
using UnityEngine;
using UnityEngine.Events;

namespace LostNotes {
	[CreateAssetMenu]
	internal sealed class GameObjectEventChannel : ScriptableObject {
		public event Action<GameObject> OnTrigger;

		[SerializeField]
		private UnityEvent<GameObject> _onTrigger;

		public void Raise(GameObject context) {
			OnTrigger?.Invoke(context);
			_onTrigger.Invoke(context);
		}
	}
}
