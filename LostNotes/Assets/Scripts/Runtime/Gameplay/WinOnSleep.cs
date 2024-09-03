using System.Collections;
using LostNotes.Level;
using UnityEngine;
using UnityEngine.Events;

namespace LostNotes.Gameplay {
	internal class WinOnSleep : MonoBehaviour, IEffectMessages {
		[SerializeField]
		private float _delay = 4f;

		[SerializeField]
		private LevelOrder _levelOrder;

		public void OnNoise(LevelGridTransform source) { }

		public void OnPush(LevelGridTransform source) { }

		public void OnPull(LevelGridTransform source) { }

		public void OnSleep(LevelGridTransform source) {
			StartCoroutine(WinAfterDelay());
		}

		private IEnumerator WinAfterDelay() {
			yield return new WaitForSecondsRealtime(_delay);
			_levelOrder.ShowWinScreen();
		}

		public void OnCalm(LevelGridTransform source) { }

		public void OnAnger(LevelGridTransform source) { }

		public void OnAttack(LevelGridTransform source) { }
	}
}
