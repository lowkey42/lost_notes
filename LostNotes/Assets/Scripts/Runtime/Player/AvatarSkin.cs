using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class AvatarSkin : MonoBehaviour, IActorMessages, INoteMessages {
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
		[SerializeField, ColorUsage(true, true)]
		private Color playColor = Color.white;
		[SerializeField, ColorUsage(true, true)]
		private Color noteColor = Color.white;

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

		public void OnStartPlaying() {
			if (_material) {
				_material.SetColor("_HighlightColor", playColor);
			}
		}

		public void OnStopPlaying() {
			if (_material) {
				_material.SetColor("_HighlightColor", turnColor);
			}
		}

		public void OnStartNote(NoteAsset note) {
			if (_material) {
				_material.SetColor("_HighlightColor", noteColor);
			}
		}

		public void OnStopNote(NoteAsset note) {
			if (_material) {
				_material.SetColor("_HighlightColor", playColor);
			}
		}
	}
}
