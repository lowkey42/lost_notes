using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class InstantiateOnClick : MonoBehaviour {
		[SerializeField, Tag]
		private string _containerLabel = "MenuContent";

		[SerializeField]
		private GameObject _prefab;

		private GameObject _instance;

		private void Awake() {
			if (TryGetComponent<Button>(out var button)) {
				button.onClick.AddListener(SetUp);
			}
		}

		private void OnDestroy() {
			TearDown();
		}

		private void SetUp() {
			var container = GameObject.FindGameObjectWithTag(_containerLabel);
			if (container && _prefab) {
				container.transform.Clear();
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
