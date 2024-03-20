using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes {
	public enum RotationTurn {
		Degrees0 = 0, Degrees90 = 90, Degrees180 = 180, Degrees270 = 270
	}

	public class Movement : MonoBehaviour {
		[SerializeField] private Level.Level _level;

		private Vector2Int Position {
			get => Vector2Int.RoundToInt(transform.position.SwizzleXZ());
			set => transform.position = value.SwizzleXZ();
		}

		protected void Start() {
			if (!_level)
				_level = GetComponentInParent<Level.Level>();
		}

		public bool MoveBy(Vector2Int delta) {
			var new2DPosition = Vector2Int.RoundToInt((transform.position + delta.SwizzleXZ()).SwizzleXZ());

			if (_level && !_level.IsWalkable(new2DPosition))
				return false;

			// TODO: lerp/animate position and report animation-completion to caller

			transform.position = new2DPosition.SwizzleXZ();
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
