using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityObject = UnityEngine.Object;

namespace LostNotes.Level {
    sealed class Level : MonoBehaviour {
	    [SerializeField] private Tilemap _interactableLayer;

	    public InteractableTile getTileAt(Vector2Int position) {
		    return null;
	    }
    }
}
