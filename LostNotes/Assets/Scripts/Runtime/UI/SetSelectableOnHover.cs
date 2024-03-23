using UnityEngine;
using UnityEngine.EventSystems;

namespace LostNotes.UI {
	internal sealed class SetSelectableOnHover : MonoBehaviour, IPointerEnterHandler {
		public void OnPointerEnter(PointerEventData eventData) {
			if (EventSystem.current) {
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}
	}
}
