using FMODUnity;
using UnityEngine;

namespace LostNotes {
	[CreateAssetMenu]
	internal sealed class FMODEventAsset : ScriptableObject {
		[SerializeField]
		private EventReference _eventToTrigger = new();

		[ContextMenu(nameof(Invoke))]
		public void Invoke() {
			if (_eventToTrigger.IsNull) {
				return;
			}

			RuntimeManager.PlayOneShot(_eventToTrigger);
		}

		public void Invoke(Transform reference) {
			if (_eventToTrigger.IsNull) {
				return;
			}

			RuntimeManager.PlayOneShot(_eventToTrigger, reference.position);
		}
	}
}
