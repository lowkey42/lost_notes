using TMPro;
using UnityEngine;

namespace LostNotes {
	[ExecuteAlways]
	internal sealed class PrintVersion : MonoBehaviour {
		[SerializeField]
		private TextMeshProUGUI _textComponent;

		private void Start() {
			UpdateText();
		}
#if UNITY_EDITOR
		private void Update() {
			UpdateText();
		}
#endif
		void UpdateText() {
			if (_textComponent) {
				_textComponent.text = $"{Application.productName} v{Application.version}";
			}
		}
	}
}
