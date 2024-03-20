using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes {
	internal sealed class Movement : MonoBehaviour {
		[SerializeField]
		private LevelComponent _level;

		private void Start() {
			if (!_level)
				_level = GetComponentInParent<LevelComponent>();
		}

		public bool MoveBy(Vector2Int delta) {
			var oldPosition = transform.position;
			var oldPosition2d = _level.WorldToGrid(oldPosition);
			var newPosition2d = oldPosition2d + delta;
			var newPosition = _level.GridToWorld(newPosition2d);

			if (_level && !_level.IsWalkable(newPosition2d)) {
				return false;
			}

			// TODO: lerp/animate position and report animation-completion to caller

			transform.position = newPosition;
			return true;
		}

		public bool MoveByLocal(Vector2Int delta) {
			var rotation = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
			return MoveBy(Vector2Int.RoundToInt((rotation * delta.SwizzleXZ()).SwizzleXZ()));
		}

		public void Rotate(RotationTurn turn) {
			transform.Rotate(Vector3.up, (int) turn);
		}
	}
}
