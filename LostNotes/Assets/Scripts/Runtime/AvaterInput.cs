using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LostNotes {
	internal sealed class AvaterInput : MonoBehaviour {
		private Vector2Int Position {
			get => Vector2Int.RoundToInt(transform.position.SwizzleXZ());
			set => transform.position = value.SwizzleXZ();
		}

		private Vector2Int _current;

		public void OnMove(InputValue value) {
			Vector2Int move = Vector2Int.RoundToInt(value.Get<Vector2>());
			Vector2Int position = Position;

			if (move.x != 0 && _current.x == 0) {
				position.x += move.x;
			}

			if (move.y != 0 && _current.y == 0) {
				position.y += move.y;
			}

			_current = move;
			Position = position;
		}
	}
}
