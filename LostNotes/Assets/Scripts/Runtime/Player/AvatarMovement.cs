using LostNotes.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostNotes.Player {
	[RequireComponent(typeof(LevelGridTransform))]
	internal sealed class AvatarMovement : MonoBehaviour, IAvatarMessages {
		[FormerlySerializedAs("_movement")]
		[SerializeField]
		private LevelGridTransform _levelGridTransform;

		[SerializeField]
		private float _stepDurationFactor = 1.0f;

		[SerializeField]
		private float _stepJumpHeight = 0.2f;

		[SerializeField]
		private int _stepJumpCount = 3;

		private void Start() {
			OnValidate();
		}

		private void OnValidate() {
			if (!_levelGridTransform)
				_levelGridTransform = GetComponentInChildren<LevelGridTransform>();
		}

		public void OnMove(Vector2Int delta) {
			_ = _levelGridTransform.MoveBy(delta, _stepJumpHeight, _stepDurationFactor, _stepJumpCount);
		}

		public void OnPlaySong(SongAsset song) {
		}

		public void OnReset() {
		}
	}
}
