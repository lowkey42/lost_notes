using FMODUnity;
using UnityEngine;

namespace LostNotes {
	[CreateAssetMenu]
	internal sealed class FMODEventAsset : ScriptableObject {
		[SerializeField]
		private EventReference eventToTrigger = new();

		[ContextMenu(nameof(Invoke))]
		public void Invoke() {
			if (eventToTrigger.IsNull) {
				return;
			}

			RuntimeManager.PlayOneShot(eventToTrigger);
		}

		public void Invoke(Transform reference) {
			if (eventToTrigger.IsNull) {
				return;
			}

			RuntimeManager.PlayOneShot(eventToTrigger, reference.position);
		}
	}
}
