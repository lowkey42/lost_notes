using LostNotes.Level;
using UnityEngine;

namespace LostNotes.Gameplay {
	/// <summary>
	///  Workaround script for enemy wall, that should fall asleep when calmed
	/// </summary>
	internal class CalmToSleep : MonoBehaviour, IEffectMessages {
		public void OnNoise(LevelGridTransform source) { }

		public void OnPush(LevelGridTransform source) { }

		public void OnPull(LevelGridTransform source) { }

		public void OnSleep(LevelGridTransform source) { }

		public void OnCalm(LevelGridTransform source) {
			gameObject.BroadcastMessage(nameof(IEffectMessages.OnSleep), source, SendMessageOptions.DontRequireReceiver);
		}

		public void OnAnger(LevelGridTransform source) { }

		public void OnAttack(LevelGridTransform source) { }
	}
}
