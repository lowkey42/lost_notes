using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class ActorSkin : MonoBehaviour, IActorMessages, ISelectionMessages {
		[SerializeField]
		private Renderer _attachedRenderer;

		private Material _material;

		private void OnEnable() {
			_material = _attachedRenderer.material;
		}

		private void OnDisable() {
			_attachedRenderer.material = _attachedRenderer.sharedMaterial;
			Destroy(_material);
		}

		[SerializeField, ColorUsage(true, true)]
		private Color _turnColor = Color.white;
		[SerializeField, ColorUsage(true, true)]
		private Color _selectionColor = Color.white;

		private bool _hasTurnStarted = false;
		private bool _isSelected = false;

		public void OnStartTurn(TurnOrder round) {
			_hasTurnStarted = true;
			UpdateColor();
		}

		public void OnEndTurn() {
			_hasTurnStarted = false;
			UpdateColor();
		}

		public void OnSelect() {
			_isSelected = true;
			UpdateColor();
		}

		public void OnDeselect() {
			_isSelected = false;
			UpdateColor();
		}

		private void UpdateColor() {
			if (_material) {
				var color = _isSelected
					? _selectionColor
					: _hasTurnStarted
						? _turnColor
						: Color.white;
				_material.SetColor("_HighlightColor", color);
			}
		}
	}
}
