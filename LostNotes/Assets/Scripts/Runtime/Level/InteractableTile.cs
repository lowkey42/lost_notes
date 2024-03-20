using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Level {
	[CreateAssetMenu(fileName = "InteractableTile", menuName = "InteractableTile", order = 0)]
	internal sealed class InteractableTile : TileBase {
		[SerializeField] private GameObject _gameObject;

		public GameObject GetGameObject() {
			return null;
		}

		public bool IsWalkable() {
			return true;
		}
	}
}
