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

		[Header("Violin")]
		[SerializeField]
		private Sprite _idleSprite;
		[SerializeField]
		private Sprite _playingSprite;
		[SerializeField]
		private Button _playButton;

		private void HandleChangeCanPlay(bool canMove) {
			_playButton.interactable = canMove;
		}

		private void HandleChangePlayState(EViolinState state) {
			_playButton.image.sprite = state switch {
				EViolinState.Idle => _idleSprite,
				EViolinState.Playing => _playingSprite,
				_ => throw new NotImplementedException(),
			};

			foreach (var button in _moveButtons) {
				button.image.sprite = state switch {
					EViolinState.Idle => _idleArrow,
					EViolinState.Playing => _playingArrow,
					_ => throw new NotImplementedException(),
				};
			}
		}

		[Header("Arrows")]
		[SerializeField]
		private Sprite _idleArrow;
		[SerializeField]
		private Sprite _playingArrow;
		[SerializeField]
		private Button[] _moveButtons = Array.Empty<Button>();

		private void HandleChangeCanMove(bool canMove) {
			foreach (var button in _moveButtons) {
				button.interactable = canMove;
			}
		}
	}
}
