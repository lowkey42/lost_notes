using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class SetImageOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler {
		[SerializeField, Tag]
		private string _containerLabel = "MenuContent";

		[SerializeField]
		private Sprite _sprite;
		public Sprite Sprite {
			get => _sprite;
			set => _sprite = value;
		}

		public void OnSelect(BaseEventData eventData) {
			TearDown();
			if (_sprite) {
				SetUp();
			}
		}

		public void OnDeselect(BaseEventData eventData) {
			TearDown();
		}

		private void OnDisable() {
			TearDown();
		}

		private void SetUp() {
			var container = GameObject.FindGameObjectWithTag(_containerLabel);
			if (container && container.TryGetComponent<Image>(out var image)) {
				image.sprite = _sprite;
				image.enabled = true;
			}
		}

		private void TearDown() {
			var container = GameObject.FindGameObjectWithTag(_containerLabel);
			if (container && container.TryGetComponent<Image>(out var image)) {
				image.sprite = null;
				image.enabled = false;
			}
		}
	}
}
