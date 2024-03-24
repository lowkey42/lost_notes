using System;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LostNotes.Gameplay;
using Random = UnityEngine.Random;

namespace LostNotes {
	internal class Hover : MonoBehaviour, IActorStatusMessages {
		[SerializeField]
		private float _heightOffset = 0.333f;

		[SerializeField]
		private float _widthOffset = 0.1f;

		[SerializeField]
		private float _duration = 2f;

		private TweenerCore<Vector3, Vector3, VectorOptions> _yTween;
		private TweenerCore<Vector3, Vector3, VectorOptions> _xTween;
		
		private void Start() {
			var delay = Random.Range(0f, 0.3f);
			_yTween = transform.DOLocalMoveY(_heightOffset, _duration + Random.Range(-.1f, 0.1f)).SetDelay(delay).SetLoops(-1, LoopType.Yoyo)
			                   .SetEase(Ease.InOutSine);
			_xTween = transform.DOLocalMoveX(_widthOffset, (_duration + Random.Range(-.1f, 0.1f)) / 2).SetDelay(delay).SetLoops(-1, LoopType.Yoyo)
			                   .SetEase(Ease.InOutSine);
		}

		public void OnGainedStatusEffect(StatusEffects gainedStatusEffect) {
			if (gainedStatusEffect.HasFlag(StatusEffects.Sleeping)) {
				_yTween.Kill();
				_xTween.Kill();
			}
		}
		
		private void OnDestroy() {
			if (_yTween.IsActive())
				_yTween.Kill();
			if (_xTween.IsActive())
				_xTween.Kill();
		}
	}
}
