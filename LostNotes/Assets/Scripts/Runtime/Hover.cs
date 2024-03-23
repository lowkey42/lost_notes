using System;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Random = UnityEngine.Random;

namespace LostNotes {
	public class Hover : MonoBehaviour {
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

		private void OnDestroy() {
			_yTween.Kill();
			_xTween.Kill();
		}
	}
}
