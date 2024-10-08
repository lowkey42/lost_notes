using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

		private TweenerCore<Color, Color, ColorOptions> _animation;
		private Material _material;

		protected void Awake() {
			if (_image) {
				_material = Instantiate(_image.material);
				_image.material = _material;
			}
		}

		protected void OnDestroy() {
			if (_material) {
				Destroy(_material);
			}

			if (_animation != null && _animation.IsActive())
				_animation.Kill();
		}

		public void OnStatusEffectsChanged(StatusEffects statusEffects) {
			if (!_image)
				return;

			var sprite = GetSprite(statusEffects);
			if (sprite) {
				_image.sprite = sprite;
				_image.gameObject.SetActive(true);
				if (statusEffects.HasFlag(StatusEffects.Sleeping)) {
					_animation ??= _material.DOFade(0.4f, 2.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
				} else if (_animation != null) {
					if (_animation.IsPlaying())
						_animation.Kill();
					_animation = null;
				}
			} else {
				_image.gameObject.SetActive(false);
			}
		}

		private Sprite GetSprite(StatusEffects statusEffects) {
			if (statusEffects.HasFlag(StatusEffects.Sleeping))
				return _sleepingSprite;
			return statusEffects.HasFlag(StatusEffects.Angry) ? _angrySprite : null;
		}
	}
}
