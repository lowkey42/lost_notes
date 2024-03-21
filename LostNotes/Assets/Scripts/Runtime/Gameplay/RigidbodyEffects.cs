using LostNotes.Level;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal sealed class RigidbodyEffects : MonoBehaviour, IEffectMessages {
		[SerializeField]
		private LevelGridTransform _gridTransform;

		public void OnNoise(LevelGridTransform source) {
		}

		public void OnPush(LevelGridTransform source) {
			var delta = _gridTransform.Position2d - source.Position2d;
			if (delta == Vector2Int.zero) {
				return;
			}

			delta = Vector2Int.RoundToInt(((Vector2) delta).normalized);

			_ = _gridTransform.MoveBy(delta);
		}

		public void OnPull(LevelGridTransform source) {
			var delta = source.Position2d - _gridTransform.Position2d;
			if (delta == Vector2Int.zero) {
				return;
			}

			delta = Vector2Int.RoundToInt(((Vector2) delta).normalized);

			_ = _gridTransform.MoveBy(delta);
		}

		public void OnSleep(LevelGridTransform source) {
		}

		public void OnCalm(LevelGridTransform source) {
		}

		public void OnAnger(LevelGridTransform source) {
		}

		public void OnAttack(LevelGridTransform source) {
		}
	}
}
