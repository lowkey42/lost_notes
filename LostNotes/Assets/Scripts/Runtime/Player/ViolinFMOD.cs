using FMODUnity;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class ViolinFMOD : MonoBehaviour {
		[SerializeField, ParamRef]
		private string _violinParameter;

		private void OnEnable() {
			AvatarInput.OnChangePlayState += AvatarInput_OnChangePlayState;
		}
		private void OnDisable() {
			AvatarInput.OnChangePlayState -= AvatarInput_OnChangePlayState;
		}

		private void AvatarInput_OnChangePlayState(EViolinState state) {
			if (!string.IsNullOrEmpty(_violinParameter)) {
				_ = RuntimeManager.StudioSystem.setParameterByNameWithLabel(_violinParameter, state.ToString());
			}
		}
	}
}
