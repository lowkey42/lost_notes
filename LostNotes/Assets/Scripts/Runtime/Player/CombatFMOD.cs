using FMOD.Studio;
using FMODUnity;
using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class CombatFMOD : MonoBehaviour, IActorMessages {
		[SerializeField, ParamRef]
		private string _combatParameter;
		[SerializeField]
		private EventReference _combatEvent = new();

		private static EventInstance _combatInstance;

		public void OnStartTurn(TurnOrder round) {
			if (!string.IsNullOrEmpty(_combatParameter)) {
				_ = RuntimeManager.StudioSystem.setParameterByNameWithLabel(_combatParameter, round.IsCurrentActorAlone ? "False" : "True");
			}

			if (!_combatEvent.IsNull && !_combatInstance.isValid()) {
				_combatInstance = RuntimeManager.CreateInstance(_combatEvent);
				_ = _combatInstance.start();
			}
		}

		public void OnEndTurn() {
		}
	}
}
