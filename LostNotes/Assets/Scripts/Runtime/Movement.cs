using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes {
	public enum RotationTurn {
		Degrees90 = 90, Degrees180 = 180, Degrees270 = 270
	}

	public class Movement : MonoBehaviour {
		[SerializeField] private Level.Level _level;

		private Vector2Int Position {
			get => Vector2Int.RoundToInt(transform.position.SwizzleXZ());
			set => transform.position = value.SwizzleXZ();
		}

		private void Start() {
			if (!_level)
				_level = GetComponentInParent<Level.Level>();
		}

		public bool MoveBy(Vector2Int delta) {
			var new2DPosition = Vector2Int.RoundToInt((transform.position + delta.SwizzleXZ()).SwizzleXZ());

			if (_level && _level.GetTileAt(new2DPosition) && !_level.GetTileAt(new2DPosition).IsWalkable())
				return false;

			transform.position = new2DPosition.SwizzleXZ();
			return true;
		}

		public bool MoveByLocal(Vector2Int delta) {
			return MoveBy(Vector2Int.RoundToInt(transform.TransformVector(new Vector3(delta.x, 0, delta.y))));
		}

		public void Rotate(RotationTurn turn) {
			transform.Rotate(Vector3.up, Mathf.Deg2Rad * (int) turn);
		}
	}
}
