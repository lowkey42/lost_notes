using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class ActorSkin : MonoBehaviour, IActorMessages {
		[SerializeField]
		private Renderer attachedRenderer;

		private Material _material;

		private void OnEnable() {
			_material = attachedRenderer.material;
		}

		private void OnDisable() {
			attachedRenderer.material = attachedRenderer.sharedMaterial;
			Destroy(_material);
		}

		[SerializeField, ColorUsage(true, true)]
		private Color turnColor = Color.white;

		public void OnStartTurn(TurnOrder round) {
			if (_material) {
				_material.SetColor("_HighlightColor", turnColor);
			}
		}

		public void OnEndTurn() {
			if (_material) {
				_material.SetColor("_HighlightColor", Color.white);
			}
		}
	}
}
