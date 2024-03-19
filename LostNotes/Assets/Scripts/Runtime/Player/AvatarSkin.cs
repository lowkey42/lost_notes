using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class AvatarSkin : MonoBehaviour, IActorMessages {
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

		public void StartTurn() {
			material.SetColor("_HighlightColor", turnColor);
		}

		public void EndTurn() {
			material.SetColor("_HighlightColor", Color.white);
		}
	}
}
