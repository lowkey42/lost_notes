using System;
using LostNotes.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class ActionPointHud : MonoBehaviour {
		[SerializeField]
		private Transform _pointContainer;
		[SerializeField]
		private Image[] _pointImages = Array.Empty<Image>();

		[SerializeField]
		private Sprite _clearSprite;
		[SerializeField]
		private Sprite _availableSprite;
		[SerializeField]
		private Sprite _highlightSprite;

		private bool _isAlone;
		private int _availablePoints;
		private int _highlightedPoints;

		private void OnEnable() {
			AvatarActor.OnChangeIsAlone += HandleIsAlone;
			AvatarActor.OnChangeAvailableActionPoints += HandleChangeActionPoints;
		}

		private void OnDisable() {
			AvatarActor.OnChangeIsAlone -= HandleIsAlone;
			AvatarActor.OnChangeAvailableActionPoints -= HandleChangeActionPoints;
		}

		private void HandleIsAlone(bool isAlone) {
			_isAlone = isAlone;
			UpdateHud();
		}

		private void HandleChangeActionPoints(int availablePoints) {
			_availablePoints = availablePoints;
			UpdateHud();
		}

		private void UpdateHud() {
			for (var i = 0; i < _pointImages.Length; i++) {
				_pointImages[i].sprite = i switch {
					_ when _highlightedPoints > i => _highlightSprite,
					_ when _availablePoints > i => _availableSprite,
					_ => _clearSprite,
				};
			}

			_pointContainer.gameObject.SetActive(!_isAlone);
		}
	}
}
