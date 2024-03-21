using System;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.Player {
	public sealed class AvatarInputInterface : MonoBehaviour {
		private void OnEnable() {
			AvatarInput.OnChangePlayState += HandleChangePlayState;
			AvatarInput.OnChangeCanMove += HandleChangeCanMove;
			AvatarInput.OnChangeCanPlay += HandleChangeCanPlay;
		}

		private void OnDisable() {
			AvatarInput.OnChangePlayState -= HandleChangePlayState;
			AvatarInput.OnChangeCanMove -= HandleChangeCanMove;
			AvatarInput.OnChangeCanPlay -= HandleChangeCanPlay;
		}
		[SerializeField]
		private Image _playImage;
		[SerializeField]
		private Button _playButton;

		private void HandleChangeCanPlay(bool canMove) {
			_playButton.interactable = canMove;
		}

		[SerializeField]
		private Sprite _idleSprite;
		[SerializeField]
		private Sprite _playingSprite;

		private void HandleChangePlayState(EViolinState state) {
			_playImage.sprite = state switch {
				EViolinState.Idle => _idleSprite,
				EViolinState.Playing => _playingSprite,
				_ => throw new NotImplementedException(),
			};
		}

		[SerializeField]
		private Button[] _moveButtons = Array.Empty<Button>();
		private void HandleChangeCanMove(bool canMove) {
			foreach (var button in _moveButtons) {
				button.interactable = canMove;
			}
		}
	}
}
