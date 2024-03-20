using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Level {
	internal sealed class LevelGridTransform : MonoBehaviour {
		[SerializeField]
		private LevelComponent _level;

		public Vector2Int Position2d => _level.WorldToGrid(transform.position);

		public int Rotation2d => Mathf.RoundToInt(transform.eulerAngles.y);

		private void Start() {
			if (!_level)
				_level = GetComponentInParent<LevelComponent>();
		}

		public bool MoveBy(Vector2Int delta) {
			var newPosition2d = Position2d + delta;
			var newPosition = _level.GridToWorld(newPosition2d);

			if (!CanMoveTo(newPosition2d))
				return false;

			// TODO: lerp/animate position and report animation-completion to caller

			transform.position = newPosition;
			return true;
		}

		public bool CanMoveTo(Vector2Int newPosition) {
			return !_level || _level.IsWalkable(newPosition);
		}

		public bool MoveByLocal(Vector2Int delta) {
			return MoveBy(LocalToWorldVector(delta));
		}

		public void Rotate(RotationTurn turn) {
			transform.Rotate(Vector3.up, (int) turn);
		}

		public Vector2Int LocalToWorldVector(Vector2Int v) {
			var rotation = Quaternion.AngleAxis(Rotation2d, Vector3.up);
			return Vector2Int.RoundToInt((rotation * v.SwizzleXZ()).SwizzleXZ());
		}

		public Vector2Int WorldToLocalVector(Vector2Int v) {
			var rotation = Quaternion.AngleAxis(-Rotation2d, Vector3.up);
			return Vector2Int.RoundToInt((rotation * v.SwizzleXZ()).SwizzleXZ());
		}

		public Vector2Int LocalToWorldPosition(Vector2Int v) {
			return LocalToWorldVector(v) + Position2d;
		}

		public Vector2Int WorldToLocalPosition(Vector2Int v) {
			return WorldToLocalVector(v - Position2d);
		}

		public Vector3 GridTo3dPosition(Vector2Int gridPosition) {
			return _level.GridToWorld(gridPosition);
		}

		public void SendMessageToObjectsInArea(TilemapMask area, string methodName, object parameter = null) {
			_level.SendMessageToObjectsInArea(this, area, methodName, parameter);
		}
	}
}
