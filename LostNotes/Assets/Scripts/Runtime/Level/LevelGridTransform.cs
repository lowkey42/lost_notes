using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LostNotes.Gameplay;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Level {
	internal sealed class LevelGridTransform : MonoBehaviour {
		[SerializeField]
		private Transform _interpolatedChild;

		[SerializeField]
		private float _interpolatedDuration = 0.25f;
		
		[SerializeField]
		private LevelComponent _level;
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _moveChannelReference;
		private GameObjectEventChannel _moveChannel;
		private Sequence _interpolationSequence;
		private float _interpolationDurationFactor = 1;

		private IEnumerator Start() {
			_ = _level || transform.TryGetComponentInParent(out _level);

			yield return _moveChannelReference.LoadAssetAsync(asset => _moveChannel = asset);
		}

		private void OnDestroy() {
			if (_moveChannel) {
				_moveChannelReference.ReleaseAsset();
			}
		}

		public Vector2Int Position2d => _level.WorldToGrid(transform.position);

		public LevelComponent Level => _level;

		public int Rotation2d => Mathf.RoundToInt(transform.eulerAngles.y);

		public LevelGridDirection Direction => (LevelGridDirection) Mathf.Clamp(Rotation2d / 90 * 90, 0, 270);

		public bool IsMoving => _interpolationSequence != null;

		public float MovingDurationFactor => _interpolationDurationFactor;
		
		public YieldInstruction MoveBy(Vector2Int delta, float jumpHeight = 0, float durationFactor = 1, int jumpCount = 1) {
			_interpolationSequence?.Kill(true);
			
			var newPosition2d = Position2d + delta;
			var newPosition = _level.GridToWorld(newPosition2d);

			if (!CanMoveTo(newPosition2d))
				return null;

			if (!_interpolatedChild) {
				transform.position = newPosition;
				SendMessageToObjectsInArea(TilemapMask.Self, nameof(ICollisionMessages.OnActorEnter), gameObject);
				if (_moveChannel)
					_moveChannel.Raise(gameObject);

				return new WaitForSeconds(durationFactor * _interpolatedDuration);
			}

			_interpolationDurationFactor = durationFactor;
			var oldPosition = transform.position;
			transform.position = newPosition;
			_interpolatedChild.position = oldPosition;
			_interpolationSequence = _interpolatedChild.DOJump(newPosition, jumpHeight, jumpCount, durationFactor * _interpolatedDuration);
			_interpolationSequence.SetEase(Ease.InOutQuad);
			_interpolationSequence.OnComplete(() => {
				_interpolatedChild.localPosition = Vector3.zero;
				SendMessageToObjectsInArea(TilemapMask.Self, nameof(ICollisionMessages.OnActorEnter), gameObject);
				if (_moveChannel) _moveChannel.Raise(gameObject);
				_interpolationSequence = null;
			});

			return _interpolationSequence.WaitForCompletion();
		}

		public bool CanMoveTo(Vector2Int newPosition) {
			return !_level || _level.IsWalkable(newPosition);
		}

		public bool CanMoveByLocal(Vector2Int delta) {
			return !_level || _level.IsWalkable(LocalToWorldPosition(delta));
		}

		public YieldInstruction MoveByLocal(Vector2Int delta, float jumpHeight = 0, float speed = 1) {
			_interpolationSequence?.Kill(true);
			return MoveBy(LocalToWorldVector(delta), jumpHeight, speed);
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
