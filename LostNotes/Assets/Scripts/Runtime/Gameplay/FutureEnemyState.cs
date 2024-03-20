using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal class FutureEnemyState {
		public readonly Enemy Enemy;
		public readonly LevelComponent Level;
		public Vector2Int Position2d;
		public int Rotation2d;

		public FutureEnemyState(LevelComponent level, Enemy enemy) {
			Level = level;
			Enemy = enemy;
			Position2d = enemy.LevelGridTransform.Position2d;
			Rotation2d = enemy.LevelGridTransform.Rotation2d;
		}

		public Vector3 Position3d => Level.GridToWorld(Position2d);

		public bool CanMoveTo(Vector2Int newPosition) {
			return !Level || Level.IsWalkable(newPosition);
		}

		public bool CanMoveBy(Vector2Int localOffset) {
			return !Level || Level.IsWalkable(LocalToWorldPosition(localOffset));
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
	}
}
