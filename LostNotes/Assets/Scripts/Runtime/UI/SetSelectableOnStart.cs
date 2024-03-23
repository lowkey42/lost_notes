using System.Collections;
using LostNotes.Level;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LostNotes.UI {
	internal sealed class SetSelectableOnStart : MonoBehaviour {
		private IEnumerator Start() {
			yield return new WaitUntil(() => LevelManager.IsReady);

			if (EventSystem.current) {
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}
	}
}
