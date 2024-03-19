using LostNotes.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

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

		public void StartTurn() {
			material.SetColor("_HighlightColor", turnColor);
		}

		public void EndTurn() {
			material.SetColor("_HighlightColor", Color.white);
		}

		public void StartPlaying() {
			material.SetColor("_HighlightColor", playColor);
		}

		public void StopPlaying() {
			material.SetColor("_HighlightColor", turnColor);
		}

		public void StartNote(InputAction action) {
			material.SetColor("_HighlightColor", noteColor);
		}

		public void StopNote(InputAction action) {
			material.SetColor("_HighlightColor", playColor);
		}
	}
}
