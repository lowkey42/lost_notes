using TMPro;
using UnityEngine;

namespace LostNotes {
	[ExecuteAlways]
	internal sealed class PrintVersion : MonoBehaviour {
		[SerializeField]
		private TextMeshProUGUI _textComponent;

		private void Update() {
			_textComponent.text = Application.version;
		}
	}
}
