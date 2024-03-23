using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace LostNotes {
	public class Hover : MonoBehaviour {
		[SerializeField]
		private float _heightOffset = 0.333f;

		[SerializeField]
		private float _widthOffset = 0.1f;

		[SerializeField]
		private float _duration = 2f;
		
		private void Start() {
			var delay = Random.Range(0f, 0.3f);
			transform.DOLocalMoveY(_heightOffset, _duration + Random.Range(-.1f, 0.1f)).SetDelay(delay).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
			transform.DOLocalMoveX(_widthOffset, (_duration + Random.Range(-.1f, 0.1f)) / 2).SetDelay(delay).SetLoops(-1, LoopType.Yoyo)
			         .SetEase(Ease.InOutSine);
		}
	}
}
