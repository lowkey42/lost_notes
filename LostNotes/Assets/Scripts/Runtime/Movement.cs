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
			var oldPosition = _level.WorldToGrid(transform.position);
			var new2DPosition = oldPosition + delta;

			if (_level && !_level.IsWalkable(new2DPosition)) {
				return false;
			}

			// TODO: lerp/animate position and report animation-completion to caller

			transform.position = _level.GridToWorld(new2DPosition);
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
