using System.Collections;
using System.Collections.Generic;
using LostNotes.Level;
using UnityEngine;
using UnityEngine.Events;

namespace LostNotes {
	[RequireComponent(typeof(LevelGridTransform))]
	public class Projectile : MonoBehaviour {
		[SerializeField]
		private LevelGridTransform _gridTransform;

		[SerializeField]
		private UnityEvent _onHit;


		public Vector2Int PushOnStart { get; set; }
		public float PushDurationFactor { get; set; }

		private void OnValidate() {
			if (_gridTransform)
				_gridTransform = GetComponentInChildren<LevelGridTransform>();
		}

		private IEnumerator Start() {
			OnValidate();
			var target = PushOnStart;
			_ = _gridTransform.Level.IsInteractionUnblocked(_gridTransform.Position2d, target, out target);
			var duration = PushDurationFactor * (PushOnStart.magnitude / target.magnitude);
			yield return _gridTransform.MoveBy(target - _gridTransform.Position2d, 0.2f, duration, 1, true);
			_onHit.Invoke();
			yield return new WaitForSeconds(0.2f);
			Destroy(gameObject);
		}
	}
}
