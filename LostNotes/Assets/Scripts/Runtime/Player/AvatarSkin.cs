using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class AvatarSkin : MonoBehaviour, IActorMessages, INoteMessages {
		[SerializeField]
		private Renderer attachedRenderer;

		private Material material;

		private void OnEnable() {
			material = attachedRenderer.material;
		}

		private void OnDisable() {
			attachedRenderer.material = attachedRenderer.sharedMaterial;
			Destroy(material);
		}

		[SerializeField, ColorUsage(true, true)]
		private Color turnColor = Color.white;
		[SerializeField, ColorUsage(true, true)]
		private Color playColor = Color.white;
		[SerializeField, ColorUsage(true, true)]
		private Color noteColor = Color.white;

		public void OnStartTurn() {
			material.SetColor("_HighlightColor", turnColor);
		}

		public void OnEndTurn() {
			material.SetColor("_HighlightColor", Color.white);
		}

		public void OnStartPlaying() {
			material.SetColor("_HighlightColor", playColor);
		}

		public void OnStopPlaying() {
			material.SetColor("_HighlightColor", turnColor);
		}

		public void OnStartNote(NoteAsset note) {
			material.SetColor("_HighlightColor", noteColor);
		}

		public void OnStopNote(NoteAsset note) {
			material.SetColor("_HighlightColor", playColor);
		}
	}
}
