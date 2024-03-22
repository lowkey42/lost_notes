using System;
using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes {
	internal class StatusSpriteTint : MonoBehaviour, IActorStatusMessages {
		[SerializeField]
		private SpriteRenderer _sprite;

		[SerializeField]
		private Color _sleepingTint = new(0.6f, 0.6f, 0.7f);

		[SerializeField]
		private Color _angryTint = new(1.0f, 0.9f, 0.9f);

		private void OnValidate() {
			if (!_sprite)
				_sprite = GetComponent<SpriteRenderer>();
		}

		public void OnStatusEffectsChanged(StatusEffects statusEffects) {
			if (statusEffects.HasFlag(StatusEffects.Sleeping))
				_sprite.color = _sleepingTint;
			if (statusEffects.HasFlag(StatusEffects.Angry))
				_sprite.color = _angryTint;
		}
	}
}
