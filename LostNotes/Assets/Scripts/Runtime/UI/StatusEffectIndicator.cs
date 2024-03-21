using LostNotes.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal class StatusEffectIndicator : MonoBehaviour, IActorStatusMessages {
		[SerializeField]
		private Image _image;

		[SerializeField]
		private Sprite _sleepingSprite;

		[SerializeField]
		private Sprite _angrySprite;

		public void OnStatusEffectsChanged(StatusEffects statusEffects) {
			if (!_image)
				return;

			var sprite = GetSprite(statusEffects);
			if (sprite) {
				_image.sprite = sprite;
				_image.gameObject.SetActive(true);
			} else
				_image.gameObject.SetActive(false);
		}

		private Sprite GetSprite(StatusEffects statusEffects) {
			if (statusEffects.HasFlag(StatusEffects.Sleeping)) return _sleepingSprite;
			if (statusEffects.HasFlag(StatusEffects.Angry)) return _angrySprite;
			return null;
		}
	}
}
