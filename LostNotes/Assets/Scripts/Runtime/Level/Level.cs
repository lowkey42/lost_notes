using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityObject = UnityEngine.Object;

namespace LostNotes.Level {
	public class InteractableTile : TileBase {
		public GameObject GetGameObject() {
			return null;
		}

		public bool IsWalkable() {
			return true;
		}
	}
    sealed class Level : MonoBehaviour {
	    [SerializeField] private Tilemap _interactableLayer;

	    public InteractableTile getTileAt(Vector2Int position) {
		    return null;
	    }
    }
}
