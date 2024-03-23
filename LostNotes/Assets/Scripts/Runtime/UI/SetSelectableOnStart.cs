using System.Collections;
using LostNotes.Level;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LostNotes.UI {
	internal sealed class SetSelectableOnStart : MonoBehaviour {

		private void OnEnable() {
			if (LevelManager.IsReady && EventSystem.current) {
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}

		private IEnumerator Start() {
			yield return new WaitUntil(() => LevelManager.IsReady);

			while (true) {
				if (EventSystem.current && !EventSystem.current.currentSelectedGameObject) {
					EventSystem.current.SetSelectedGameObject(gameObject);
				}

				yield return null;
			}
		}
	}
}
