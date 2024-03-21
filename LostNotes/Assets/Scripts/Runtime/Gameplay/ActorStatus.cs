using System;
using LostNotes.Level;
using MyBox;
using UnityEngine;

namespace LostNotes.Gameplay {
	internal class ActorStatus : MonoBehaviour, IEffectMessages {
		[SerializeField]
		private StatusEffects _statusEffects;

		private void Start() {
			gameObject.BroadcastMessage(nameof(IActorStatusMessages.OnStatusEffectsChanged), _statusEffects, SendMessageOptions.DontRequireReceiver);
		}

		[ButtonMethod]
		private void DebugMakeAngry() {
			ApplyStatusEffect(StatusEffects.Angry);
		}

		public StatusEffects GetStatusFlags() {
			return _statusEffects;
		}

		public bool HasStatusEffect(StatusEffects status) {
			return GetStatusFlags().HasFlag(status);
		}

		public void ApplyStatusEffect(StatusEffects status) {
			var newStatusEffects = status & ~_statusEffects;
			if (newStatusEffects == 0)
				return;

			_statusEffects |= newStatusEffects;
			gameObject.BroadcastMessage(nameof(IActorStatusMessages.OnGainedStatusEffect),   newStatusEffects, SendMessageOptions.DontRequireReceiver);
			gameObject.BroadcastMessage(nameof(IActorStatusMessages.OnStatusEffectsChanged), _statusEffects,   SendMessageOptions.DontRequireReceiver);
		}

		public void RemoveStatusEffect(StatusEffects status) {
			var lostStatusEffects = status & _statusEffects;
			if (lostStatusEffects == 0)
				return;

			_statusEffects &= ~lostStatusEffects;
			gameObject.BroadcastMessage(nameof(IActorStatusMessages.OnLostStatusEffect),     lostStatusEffects, SendMessageOptions.DontRequireReceiver);
			gameObject.BroadcastMessage(nameof(IActorStatusMessages.OnStatusEffectsChanged), _statusEffects,    SendMessageOptions.DontRequireReceiver);
		}

		public void OnNoise(LevelGridTransform source) { }

		public void OnPush(LevelGridTransform source) { }

		public void OnPull(LevelGridTransform source) { }

		public void OnSleep(LevelGridTransform source) {
			if (!HasStatusEffect(StatusEffects.Angry))
				ApplyStatusEffect(StatusEffects.Sleeping);
		}

		public void OnCalm(LevelGridTransform source) {
			if (!HasStatusEffect(StatusEffects.Sleeping))
				RemoveStatusEffect(StatusEffects.Angry);
		}

		public void OnAnger(LevelGridTransform source) {
			if (!HasStatusEffect(StatusEffects.Sleeping))
				ApplyStatusEffect(StatusEffects.Angry);
		}

		public void OnAttack(LevelGridTransform source) {
			ApplyStatusEffect(StatusEffects.Sleeping);
		}
	}
}
