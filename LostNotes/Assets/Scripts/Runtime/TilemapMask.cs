using System;
using UnityEngine;

namespace LostNotes {
	[Serializable]
	public class TilemapMask {
		[SerializeField] private bool[] _mask;

		public TilemapMask(Vector2Int size) {
			Size  = size;
			_mask = new bool[size.x * size.y];
		}

		public Vector2Int Size { get; private set; }

		public void Resize(Vector2Int newSize) {
			if (newSize.x == Size.x && newSize.y == Size.y)
				return; // unchanged

			var newMask = new bool[newSize.x * newSize.y];
			for (var x = 0; x < Math.Min(newSize.x, Size.x); ++x) {
				for (var y = 0; y < Math.Min(newSize.y, Size.y); ++y)
					newMask[x + (y * newSize.x)] = _mask[x + (y * Size.x)];
			}

			_mask = newMask;
			Size  = newSize;
		}

		public bool IsSet(Vector2Int position) {
			if (position.x < Size.x && position.y < Size.y)
				return _mask[position.x + (position.y * Size.x)];

			return false;
		}

		public void Set(Vector2Int position, bool set) {
			if (position.x < Size.x && position.y < Size.y)
				_mask[position.x + (position.y * Size.x)] = set;
		}
	}
}
