using LostNotes.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LostNotes.UI {
	internal sealed class SongButton : MonoBehaviour, ISongMessages {
		[SerializeField]
		private Image _image;

		public void OnSetSong(SongAsset song) {
			if (song.IsAvailable) {
				_image.sprite = song.HudSprite;

				if (TryGetComponent<SetImageOnSelect>(out var image)) {
					image.Sprite = song.DetailSprite;
				}
			}
		}
	}
}
