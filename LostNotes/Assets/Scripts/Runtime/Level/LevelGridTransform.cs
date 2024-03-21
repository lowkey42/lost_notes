using System.Collections;
using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Level {
	internal sealed class LevelGridTransform : MonoBehaviour {
		[SerializeField]
		private LevelComponent _level;
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> moveChannelReference;
		private GameObjectEventChannel moveChannel;

		private IEnumerator Start() {
			_ = _level || transform.TryGetComponentInParent(out _level);

			var handle = moveChannelReference.LoadAssetAsync();
			yield return handle;
			moveChannel = handle.Result;
		}

		public Vector2Int Position2d => _level.WorldToGrid(transform.position);

		public LevelComponent Level => _level;

		public int Rotation2d => Mathf.RoundToInt(transform.eulerAngles.y);

		public bool MoveBy(Vector2Int delta) {
			var newPosition2d = Position2d + delta;
			var newPosition = _level.GridToWorld(newPosition2d);

			if (!CanMoveTo(newPosition2d))
				return false;

			// TODO: lerp/animate position and report animation-completion to caller

			transform.position = newPosition;

			if (moveChannel) {
				moveChannel.Raise(gameObject);
			}

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

		public IEnumerable<GameObject> ObjectsInArea(TilemapMask area) {
			return _level.ObjectsInArea(this, area);
		}

		public void SendMessageToObjectsInArea(TilemapMask area, string methodName, object parameter = null) {
			_level.SendMessageToObjectsInArea(this, area, methodName, parameter);
		}
	}
}
