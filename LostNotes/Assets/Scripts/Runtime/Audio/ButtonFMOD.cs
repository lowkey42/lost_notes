using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LostNotes.Audio {
	internal sealed class ButtonFMOD : MonoBehaviour, ISelectHandler {
		[SerializeField]
		private EventReference _onHover = new();

		public void OnSelect(BaseEventData eventData) {
			RuntimeManager.PlayOneShot(_onHover);
		}

		[SerializeField]
		private EventReference _onSelect = new();

		private void Awake() {
			if (TryGetComponent<Button>(out var button)) {
				button.onClick.AddListener(() => RuntimeManager.PlayOneShot(_onSelect));
			}
		}
	}
}
