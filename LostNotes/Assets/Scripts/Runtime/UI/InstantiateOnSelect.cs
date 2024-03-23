using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LostNotes.UI {
	internal sealed class InstantiateOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler {
		[SerializeField, Tag]
		private string _containerLabel = "MenuContent";

		[SerializeField]
		private GameObject _prefab;

		private GameObject _instance;

		public void OnSelect(BaseEventData eventData) {
			TearDown();
			if (_prefab) {
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
			if (container && _prefab) {
				_instance = Instantiate(_prefab, container.transform);
			}
		}

		private void TearDown() {
			if (_instance) {
				Destroy(_instance);
				_instance = null;
			}
		}
	}
}
