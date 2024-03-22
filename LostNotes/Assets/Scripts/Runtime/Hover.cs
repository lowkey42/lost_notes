using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace LostNotes {
	public class Hover : MonoBehaviour {
		private void Start() {
			transform.DOMoveY(0.333f, 2f).SetDelay(Random.Range(0f, 1f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
		}
	}
}
