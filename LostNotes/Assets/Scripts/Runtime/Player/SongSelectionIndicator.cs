using System.Collections.Generic;
using LostNotes.Gameplay;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LostNotes.Player {
	internal sealed class SongSelectionIndicator : MonoBehaviour {
		[SerializeField]
		private AvatarActor _avatar;
		[SerializeField]
		private TileBase _indicatorTile;

		private void OnEnable() {
			AvatarInput.OnChangePlayState += AvatarInput_OnChangePlayState;
		}

		private void OnDisable() {
			AvatarInput.OnChangePlayState -= AvatarInput_OnChangePlayState;
		}

		private readonly List<Vector2Int> _selectedTiles = new();
		private readonly List<GameObject> _selectedObjects = new();

		private void AvatarInput_OnChangePlayState(EViolinState state) {
			switch (state) {
				case EViolinState.Playing:
					CreateIndicators();
					break;
				case EViolinState.Idle:
					ClearIndicators();
					break;
			}
		}

		private void CreateIndicators() {
			_avatar.Level.SetHighlighting(_avatar.TilesInSongRange, _indicatorTile);
			_selectedTiles.AddRange(_avatar.TilesInSongRange);

			foreach (var obj in _avatar.ObjectsInSongRange) {
				obj.BroadcastMessage(nameof(ISelectionMessages.OnSelect), SendMessageOptions.DontRequireReceiver);
				_selectedObjects.Add(obj);
			}
		}

		private void ClearIndicators() {
			_avatar.Level.ClearHighlighting(_selectedTiles);
			_selectedTiles.Clear();

			foreach (var obj in _selectedObjects) {
				if (obj) {
					obj.BroadcastMessage(nameof(ISelectionMessages.OnDeselect), SendMessageOptions.DontRequireReceiver);
				}
			}

			_selectedObjects.Clear();
		}
	}
}
